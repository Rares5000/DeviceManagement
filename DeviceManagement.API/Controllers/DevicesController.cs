using System.Security.Claims;
using DeviceManagement.Core.DTOs;
using DeviceManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDto>>> GetAll()
    {
        var devices = await _deviceService.GetAllAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetById(int id)
    {
        try
        {
            var device = await _deviceService.GetByIdAsync(id);
            return Ok(device);
        } catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<DeviceDto>> Create([FromBody] CreateDeviceDto deviceDto)
    {
        try
        {
            var device = await _deviceService.CreateAsync(deviceDto);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        } catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DeviceDto>> Update(int id, [FromBody] UpdateDeviceDto deviceDto)
    {
        try
        {
            var device = await _deviceService.UpdateAsync(id, deviceDto);
            return Ok(device);
        } catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        } catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _deviceService.DeleteAsync(id);
            return NoContent();
        } catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id}/assign")]
    public async Task<ActionResult<DeviceDto>> Assign(int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var device = await _deviceService.AssignAsync(id, userId);
            return Ok(device);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("{id}/unassign")]
    public async Task<ActionResult<DeviceDto>> Unassign(int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var device = await _deviceService.UnassignAsync(id, userId);
            return Ok(device);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("{id}/generate-description")]
    public async Task<ActionResult<DeviceDto>> GenerateDescription(int id)
    {
        try
        {
            var device = await _deviceService.GenerateDescriptionAsync(id);
            return Ok(device);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("credit balance"))
        {
            return StatusCode(503, "AI service unavailable - insufficient credits.");
        }
        catch (Exception)
        {
            return StatusCode(503, "AI service temporarily unavailable.");
        }
    }
}