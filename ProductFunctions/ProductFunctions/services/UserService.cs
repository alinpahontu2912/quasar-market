using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreFunctions
{
  internal class UserService
  {
    public List<User> GetAllUsers()
    {

      List<User> allUsers = new();
      try
      {
        using (var dataBase = new TrainingAlinContext())
        {
          allUsers = dataBase.Users.Select(user => user).ToList();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return allUsers;
    }

    public async Task<User> LogInUser(string email, string password)
    {
      User wantedUser = null;
      using (var dataBase = new TrainingAlinContext())
      {
        wantedUser = await dataBase.Users.Where(user => user.Email == email).FirstAsync();
        Console.Write(wantedUser.HashPass);
        Console.Write(Utils.hashPassword(password, wantedUser.HashSalt));
        if (wantedUser.HashPass == Utils.hashPassword(password, wantedUser.HashSalt))
        {
          return wantedUser;
        }
      }
      return null;
    }

    public async Task<bool> AddNewUser(User newUser)
    {
      using (var dataBase = new TrainingAlinContext())
      {
        try
        {
          Utils.secureUserFields(newUser);
          if (dataBase.Users.FirstOrDefaultAsync(user => user.Email == newUser.Email) != null) {
            return false;
          }
          dataBase.Users.Add(newUser);
          await dataBase.SaveChangesAsync();
          return true;
        }
        catch (Exception ex)
        {
          return false;
        }
      }


    }


    public async Task<User> GetNewRefreshToken(HttpRequest req)
    {
      ValidateJWT validate = new(req);
      User wantedUser = null;
      using (var dataBase = new TrainingAlinContext())
      {
        if (validate.IsValid)
        {
          wantedUser = await dataBase.Users.Where(user => user.Email == validate.Username).FirstAsync();
          if (wantedUser.RefreshToken == validate.RefreshToken)
          {
            wantedUser.RefreshToken = Utils.GenerateRefreshToken();
            await dataBase.SaveChangesAsync();
            return wantedUser;
          }
        }
      }
      return null;
    }


  }
}
