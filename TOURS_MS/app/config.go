package app

import "os"

var (
	ConnectionString string
	Port             string
)

func Init() {
	ConnectionString = "host=" + os.Getenv("DB_HOST") +
		" user=" + os.Getenv("DB_USER") +
		" password=" + os.Getenv("DB_PASSWORD") +
		" dbname=" + os.Getenv("DB_DATABASE") +
		" port=" + "postgres" + os.Getenv("DB_PORT") +
		" sslmode=disable"

	Port = ":8000"
}

/*
const (
	ConnectionString = "host=" + os.LookupEnv("DB_HOST") + "user=" + os.Getenv("DB_USER") + " password=" + os.Getenv("DB_PASSWORD") + "dbname=" + os.Getenv("DB_DATABASE") + "port=" + os.Getenv("DB_PORT") + "sslmode=disable"
	Port             = ":8000"
)*/
