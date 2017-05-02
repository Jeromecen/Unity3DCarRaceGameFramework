using System.Collections;
using System.Collections.Generic;

public class LoginRoleInfo : Singleton<LoginRoleInfo>{
    public LoginRoleInfo()
    {
        FreshRoleInfo();
    }
    private int roleID;
    public int RoleID
    {
        set;
        get;
    }
    List<int> mCars = new List<int>();

    private string name;
    public string Name
    {
        set;
        get;
    }

    public List<int> GetCars()
    {
        return mCars;
    }

    // 拥有的车，来自网络
    public void FreshRoleInfo()
    {
        RoleID = 1;
        Name = "jack";
        mCars.Add(1);
    }
}
