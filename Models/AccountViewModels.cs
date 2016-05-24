﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Enterprise.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Enterprise.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "Електронна адреса")]
		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel
	{
		public string ReturnUrl { get; set; }
	}

	public class SendCodeViewModel
	{
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
		public string ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}

	public class VerifyCodeViewModel
	{
		[Required]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Код")]
		public string Code { get; set; }
		public string ReturnUrl { get; set; }

		[Display(Name = "Запам'ятати цей браузер?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }
	}

	public class ForgotViewModel
	{
		[Required]
		[Display(Name = "Електронна адреса")]
		public string Email { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Логін")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Display(Name = "Запам'ятати мене?")]
		public bool RememberMe { get; set; }
	}

	public class UserViewModel
	{
		[Required]
		[Display(Name = "Логін")]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Електронна адреса")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Повне ім'я")]
		[RegularExpression("^([a-zA-Zа-яА-ЯіІїЇ' ]){2,50}", ErrorMessage = "Строка імені містить недопустимі символи або її довжина не входить в діапазон від 2 до 50 символів.")]
		public string FullName { get; set; }

		[Display(Name = "Дата народження")]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }

		[Display(Name = "Роль")]
		public string Role { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "{0} повинен містити хоча б {2} символів.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Підтвердження пароля")]
		[Compare("Password", ErrorMessage = "Пароль і підтвердження не співпадають.")]
		public string ConfirmPassword { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Електронна адреса")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "{0} повинен містити хоча б {2} символів.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Підтвердження пароля")]
		[Compare("Password", ErrorMessage = "Пароль і підтвердження не співпадають.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Електронна адреса")]
		public string Email { get; set; }
	}
}