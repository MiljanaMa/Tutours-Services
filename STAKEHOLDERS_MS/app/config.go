package app

import "os"

var (
	ConnectionString string
	Port             string
)

func Init() {
	ConnectionString =
		"user=" + os.Getenv("DB_USER_S") +
			" password=" + os.Getenv("DB_PASSWORD_S") +
			" host=" + os.Getenv("DB_HOST_S") +
			" search_path=public" +
			" dbname=" + os.Getenv("DB_DATABASE_S") +
			" port=" + os.Getenv("DB_PORT_S") +
			" sslmode=disable"
	//ConnectionString = "host=localhost user=postgres password=super dbname=stakeholders port=5432 sslmode=disable"
	Port = ":8097"
}
