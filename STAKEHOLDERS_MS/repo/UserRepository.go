package repo

import (
	"fmt"
	"gorm.io/gorm"
	"stakeholder/model"
)

type UserRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *UserRepository) GetActiveByUsername(username string) (model.User, error) {
	var user model.User
	fmt.Println("username " + username)
	dbResult := repo.DatabaseConnection.Where("username = ?", username).Find(&user)
	if dbResult.Error != nil {
		return user, dbResult.Error
	}
	return user, nil
}
func (repo *UserRepository) GetPerson(id int) (model.Person, error) {
	var person model.Person
	dbResult := repo.DatabaseConnection.Where("user_id = ?", id).Find(&person)
	if dbResult.Error != nil {
		return person, dbResult.Error
	}
	return person, nil
}

func (repo *UserRepository) Update(user model.Person) error {
	dbResult := repo.DatabaseConnection.Save(user)
	return dbResult.Error
}
