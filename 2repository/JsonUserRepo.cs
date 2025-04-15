using modals;
using System.Text.Json;


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
                File.Create(_filePath).Close();
            }
            using FileStream stream = File.OpenRead(_filePath);
            return JsonSerializer.Deserialize<List<Users>>(stream) ?? new List<Users>();

        }
        catch
        {
            throw new Exception("Error loading users from file");
        }
    }

    public void SaveUsers()
    {
        string json = JsonSerializer.Serialize(_users);
        File.WriteAllText(_filePath, json);
    }
}
