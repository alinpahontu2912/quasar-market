using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StoreFunctions
{
  public partial class User
  {
    public string Email { get; set; }
    public string FullName { get; set; }
    public string HashPass { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int Role { get; set; }
    public string HashSalt { get; set; }
    public string RefreshToken { get; set; }

    public User()
    {
      Role = 0;
    }

  }
}
