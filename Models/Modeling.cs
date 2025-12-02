namespace api.Models
{
    public struct Modeling
    {
        public struct Database
        {
            public struct PSQL
            {
                public struct MAIN
                {
                    public struct CONNECTION
                    {
                        public struct MAIN
                        {
                            public override string ToString() => "Host=localhost;Port=5432;Database=company_management;Username=admin;Password=TEST001; Maximum Pool Size=1000;";
                        }
                    }
                    public struct PRC
                    {
                        public struct INIT
                        {
                            public struct GETTING
                            {
                                public readonly int SERVER_VERSION = 1000;
                                public readonly int ADMIN_LOGIN = 1001;

                                public GETTING() { }
                            }
                        }
                        public struct ADMIN
                        {
                            public struct GETTING
                            {
                                public readonly int SERVER_VERSION = 1000;
                                public GETTING() { }
                            }
                            public override string ToString() => "CALL public.prc_admin(@mode, @in_data, null)";
                        }
                    }
                }
            }
        }
    }
}
