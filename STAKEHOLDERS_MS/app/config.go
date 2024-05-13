package app

var (
	ConnectionString string
	Port             string
)

func Init() {
	/*ConnectionString =
	"user=" + os.Getenv("DB_USER") +
		" password=" + os.Getenv("DB_PASSWORD") +
		" host=" + os.Getenv("DB_HOST") +
		" search_path=public" +
		" dbname=" + os.Getenv("DB_DATABASE") +
		" port=" + os.Getenv("DB_PORT") +
		" sslmode=disable"*/
	ConnectionString = "host=localhost user=postgres password=super dbname=stakeholders port=5432 sslmode=disable"
	Port = ":8097"
}
