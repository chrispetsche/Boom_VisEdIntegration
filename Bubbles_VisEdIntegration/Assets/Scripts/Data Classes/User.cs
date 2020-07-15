using System.Collections;
using System.Collections.Generic;

public class User
{
	public int userId;
	public string email;
    public string firstName;
	public string lastName;
	public char[] initials;
    public byte[] image;
	public List<Project> projects;
	public UserType userType;

	public User()
	{
		userId = 0;
		email = "";
		firstName = "";
		lastName = "";
		initials = new char[2];
        image = new byte[0];
		projects = new List<Project>();
		userType = UserType.Null;
	}

	public User(int _userId, string _email, string _firstName, string _lastName, char[] _initials, List<Project> _projects, UserType _userType)
	{
		userId = _userId;
		email = _email;
		firstName = _firstName;
		lastName = _lastName;
		initials = _initials;
		projects = _projects;
		userType = _userType;
        image = new byte[0];
    }
}
