using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models.Dtos;
using AutoMapper;
using Domain;
using Domain.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("{param}", Name = nameof(GetUserAsync))]
        public async Task<IActionResult> GetUserAsync(string param)
        {
            var user = int.TryParse(param, out var d)
                ? await _userService.GetUserByIdAsync(d)
                : await _userService.GetUserByNameAsync(param);

            if (user == null)
                return NotFound();
            var userToReturn = _mapper.Map<UserForListDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UsersResourceParameters resourceParameters)
        {
            var users = await _userService.GetUsersAsync(resourceParameters);

            if (users == null)
                return NotFound();

            var previousPageLink = users.HasPrevious
                ? CreateUsersResourceUri(resourceParameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = users.HasNext
                ? CreateUsersResourceUri(resourceParameters, ResourceUriType.NextPage) : null;

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            if (!await _userService.UserExistsAsync(id))
                return NotFound();

            await _userService.DeleteUserAsync(id);

            if (await _userService.SaveChangesAsync())
                return Ok();

            return BadRequest("Deleting user failed");
        }

        [HttpPut("userId")]
        public async Task<IActionResult> UpdateUserAsync(int userId, UserForEditionDto userForEditionDto)
        {
            var userFromRepo = await _userService.GetUserByIdAsync(userId);

            if (userFromRepo == null)
                return NotFound();

            _mapper.Map(userForEditionDto, userFromRepo);

            if (await _userService.SaveChangesAsync())
                return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> PartiallyUpdateUserAsync(int userId, JsonPatchDocument<UserForEditionDto> patchDocument)
        {
            var userFromRepo = await _userService.GetUserByIdAsync(userId);
            if (userFromRepo == null)
                return NotFound();

            var userToPatch = _mapper.Map<UserForEditionDto>(userFromRepo);

            patchDocument.ApplyTo(userToPatch, ModelState);

            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userFromRepo);

            if (!await _userService.SaveChangesAsync())
                return BadRequest();

            return NoContent();
        }

        private string CreateUsersResourceUri(UsersResourceParameters usersResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetUsersAsync",
                        new
                        {
                            pageNumber = usersResourceParameters.PageNumber - 1,
                            pageSize = usersResourceParameters.PageSize,
                            searchQuery = usersResourceParameters.SearchQuery,
                            role = usersResourceParameters.Role
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetUsersAsync",
                        new
                        {
                            pageNumber = usersResourceParameters.PageNumber + 1,
                            pageSize = usersResourceParameters.PageSize,
                            searchQuery = usersResourceParameters.SearchQuery,
                            role = usersResourceParameters.Role
                        });
                default:
                    return Url.Link("GetUsersAsync",
                        new
                        {
                            pageNumber = usersResourceParameters.PageNumber,
                            pageSize = usersResourceParameters.PageSize,
                            searchQuery = usersResourceParameters.SearchQuery,
                            role = usersResourceParameters.Role
                        });
            }
        }
    }
}