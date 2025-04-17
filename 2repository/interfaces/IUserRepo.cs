using modals;
namespace repository;

public interface IUserRepo
{
    Users AddUser(Users user);
    Users GetUser(string username);

    List<Users> LoadUsers();
    void SaveUsers();
    void DeleteUser(string username);

}