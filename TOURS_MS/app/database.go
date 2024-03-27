package app

import (
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"log"
)

func InitDB() *gorm.DB {

	database, err := gorm.Open(postgres.Open(ConnectionString), &gorm.Config{SkipDefaultTransaction: true})

	if err != nil {
		log.Fatal(err)
	}

	return database
}
