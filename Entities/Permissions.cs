namespace BaseRMS.Entities;

public static class Permissions
{
    public static class User
    {
        public const string Create = "user.create";
        public const string Edit = "user.edit";
        public const string Delete = "user.delete";
        public const string View = "user.view";
        public const string List = "user.list";
    }

    public static class Role
    {
        public const string Create = "role.create";
        public const string Edit = "role.edit";
        public const string Delete = "role.delete";
        public const string View = "role.view";
    }

    public static class MFA
    {
        public const string Disable = "mfa.disable";
    }

    public static class Client
    {
        public const string Create = "client.create";
        public const string Edit = "client.edit";
        public const string Delete = "client.delete";
        public const string View = "client.view";
    }
     public static class Contract
    {
        public const string Create = "contract.create";
        public const string Edit = "contract.edit";
        public const string Delete = "contract.delete";
        public const string View = "contract.view";
    }

}
