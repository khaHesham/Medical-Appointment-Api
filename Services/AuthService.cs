using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicalAppointment.API.Models;
using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.Exceptions;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using MedicalAppointmentApi.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace MedicalAppointmentApi.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var userRepo = _unitOfWork.Repository<User>();
            var user = userRepo.FindAsync(u => u.Email == request.Email).Result;
            // check if user exists and has valid password
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid email or password");

            return GenerateToken(user);
        }

        public Task<bool?> RegisterAsync(RegisterRequest request, Role role)
        {
            var userRepo = _unitOfWork.Repository<User>();
            var patientRepo = _unitOfWork.Repository<Patient>();
            var doctorRepo = _unitOfWork.Repository<Doctor>();

            // search if same email exists
            User? user = userRepo.FindAsync(u => u.Email == request.Email).Result;
            if (user != null)
            {
                throw new BadRequestException("Email already exists");
            }

            // hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // insert in the correct table
            switch (role)
            {
                case Role.Patient:
                    var patient = new Patient
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        Password = hashedPassword,
                        UserRole = role,
                    };
                    patientRepo.AddAsync(patient);
                    break;

                case Role.Doctor:
                    var doctor = new Doctor
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        Password = hashedPassword,
                        UserRole = role,
                    };
                    doctorRepo.AddAsync(doctor);
                    break;

                default:
                    throw new ArgumentException("Invalid role");
            }

            // save changes
            var result = _unitOfWork.CompleteAsync().Result;
            return (result > 0) ? Task.FromResult<bool?>(true) : Task.FromResult<bool?>(false);

        }

        // helper method to generate JWT token
        private Task<LoginResponse?> GenerateToken(User user)
        {
            var jwtSecret = _config["JWT_SECRET"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");
            var key = Encoding.ASCII.GetBytes(jwtSecret!);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult<LoginResponse?>(new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                Role = user.UserRole.ToString(),
            });
        }
    }

}