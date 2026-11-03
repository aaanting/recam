using System;
using System.ComponentModel.DataAnnotations;

namespace Remp.Service.DTOs;

public class UserRegisterDto
{
  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  [MaxLength(100)]
  public string PhotographyCompanyName { get; set; } = string.Empty;

  [Required]
  [DataType(DataType.Password)]
  public string Password { get; set; } = string.Empty;

  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
  public string ConfirmPassword { get; set; } = string.Empty;
}
