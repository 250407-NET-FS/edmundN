using modals;
using System.Text.Json;
using System.IO;

namespace repository;

public class JsonUserRepo : IUserRepo
{
    private List<Users> _users;
    private string _filePath;

    public JsonUserRepo()
    {
        _filePath = Path.Combine("./4data/users.json");
        _users = new List<Users>();
        LoadUsers();
    }

    public Users AddUser(Users user)
    {
        _users.Add(user);
        SaveUsers();
        return user;
    }

    public Users GetUser(string username)
    {
        return _users.Find(u => u.Username == username);
    }

    public Users? GetByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username == username);
    }

    public void UpdateUser(Users user)
    {
        int index = _users.FindIndex(u => u.Username == user.Username);
        if (index != -1)
        {
            _users[index] = user;
            SaveUsers();
        }
    }

    public void DeleteUser(string username)
    {
        _users.RemoveAll(u => u.Username == username);
        SaveUsers();
    }

    public List<Users> LoadUsers()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                // Create the file if it does not exist
                using (File.Create(_filePath)) { }
                return new List<Users>();
            }

            using FileStream stream = File.OpenRead(_filePath);
            var users = JsonSerializer.Deserialize<List<Users>>(stream);
            _users = users ?? new List<Users>();
            return _users;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error loading users from file: {ex.Message}", ex);
        }
    }

    public void SaveUsers()
    {
        try
        {
            string json = JsonSerializer.Serialize(_users);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {

            throw new Exception($"Error saving users to file: {ex.Message}", ex);
        }
    }
}