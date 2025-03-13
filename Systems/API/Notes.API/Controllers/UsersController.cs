using Microsoft.AspNetCore.Mvc;
using Notes.API.Models;
using Notes.Services.User;
using AutoMapper;

namespace Notes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    // GET: api/Users/{userId}
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
    {
        var userModel = await _userService.GetByIdAsync(userId);
        if (userModel == null)
        {
            return NotFound();
        }
        var response = _mapper.Map<UserResponse>(userModel);
        return Ok(response);
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        var responses = _mapper.Map<IEnumerable<UserResponse>>(users);
        return Ok(responses);
    }

    // DELETE: api/Users/{userId}
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteAsync(userId);
        return NoContent();
    }
}
