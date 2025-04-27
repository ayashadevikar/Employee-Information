using FullStackCrud.Server.Helpers;
using FullStackCrud.Server.Models;
using FullStackCrud.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackCrud.Server.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly UserService _userService;
        private readonly IConfiguration _config;


        public EmployeeController(EmployeeService employeeService, UserService userService, IConfiguration config)
        {
            _employeeService = employeeService;
            _userService = userService;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get() =>
            await _employeeService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            var employee = await _employeeService.GetAsync(id);
            if (employee == null)
                return NotFound();

            return employee;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            await _employeeService.CreateAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Employee employee)
        {
            var existing = await _employeeService.GetAsync(id);
            if (existing == null)
                return NotFound();

            employee.Id = existing.Id;
            await _employeeService.UpdateAsync(id, employee);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _employeeService.GetAsync(id);
            if (existing == null)
                return NotFound();

            await _employeeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User request)
        {
            var existingUser = await _userService.GetUserAsync(request.Username);
            if (existingUser != null)
                return BadRequest("User already exists");

            //if (!BCrypt.Net.BCrypt.Verify(request.PasswordHash, request.PasswordHash))
            //{
            //    request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            //}

            request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            await _userService.CreateUserAsync(request);
            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email); // Modify to search by email
            if (user == null)
                return Unauthorized("Invalid email");

            // Verify plain password with hashed password from database
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) // Make sure to compare password not password hash
                return Unauthorized("Invalid password");

            var token = JwtHelper.GenerateJwtToken(
                user.Email, // Use email for token generation
                _config["Jwt:Key"],
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"]);

            var tokenResponse = new TokenResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(2)
            };

            return Ok(tokenResponse);
        }
        //[HttpPost("register")]
        //public IActionResult Register(User request)
        //{
        //    if (_userService.GetUser(request.Username) != null)
        //        return BadRequest("User already exists");

        //    // Hash the password
        //    request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);


        //    _userService.CreateUser(request);
        //    return Ok("User registered successfully");
        //}

        //[HttpPost("login")]
        //public IActionResult Login(User request)
        //{
        //    var user = _userService.GetUser(request.Username);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
        //        return Unauthorized("Invalid credentials");

        //    var token = JwtHelper.GenerateJwtToken(
        //        request.Username,
        //        _config["Jwt:Key"],
        //        _config["Jwt:Issuer"],
        //        _config["Jwt:Audience"]);

        //    return Ok(new { token });
        //}
    }

    }

