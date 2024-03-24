package service

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"strconv"
)

type EncounterService struct {
	EncounterRepo *repo.EncounterRepository
}

func (service *EncounterService) GetApproved() ([]*model.Encounter, error) {

	if encounters, err := service.EncounterRepo.GetApproved(); err == nil {
		return encounters, nil
	}

	return nil, fmt.Errorf("Couldn't find any.")

}

func (service *EncounterService) GetApprovedByStatus(status model.EncounterStatus) ([]*model.Encounter, error) {
	if encounters, err := service.EncounterRepo.GetApprovedByStatus(status); err == nil {
		return encounters, nil
	}
	return nil, fmt.Errorf("Couldn't find any.")

}

func (service *EncounterService) GetByUser(userId int) ([]*model.Encounter, error) {
	if encounters, err := service.EncounterRepo.GetByUser(userId); err == nil {
		return encounters, nil
	}
	return nil, fmt.Errorf("Couldn't find any.")
}

func (service *EncounterService) GetTouristCreatedEncounters() ([]*model.Encounter, error) {
	if encounters, err := service.EncounterRepo.GetTouristCreatedEncounters(); err == nil {
		return encounters, nil
	}
	return nil, fmt.Errorf("Couldn't find any.")
}

func (service *EncounterService) GetNearbyHidden(userId int) ([]*model.Encounter, error) {

	lat, long, err := getLatLong(userId)

	if err != nil {
		return nil, err
	}

	if encounters, err := service.EncounterRepo.GetByType(long, lat, model.EncounterType("LOCATION")); err == nil {
		encounters = findInVicinity(&encounters, lat, long)
		return encounters, nil
	}
	return nil, fmt.Errorf("Couldn't find any.")
}

func (service *EncounterService) GetNearby(userId int) ([]*model.Encounter, error) {
	lat, long, err := getLatLong(userId)
	if err != nil {
		return nil, err
	}

	if encounters, err := service.EncounterRepo.GetAll(); err == nil {

		encounters = findInVicinity(&encounters, lat, long)

		return encounters, nil
	}

	return nil, fmt.Errorf("Couldn't find any.")
}

func (service *EncounterService) Create(encounter *model.Encounter) (*model.Encounter, error) {

	if res, err := service.EncounterRepo.Create(encounter); err == nil {
		return res, nil
	}
	return nil, fmt.Errorf("Couldn't create.")

}

func (service *EncounterService) Delete(id int) error {
	if err := service.EncounterRepo.Delete(id); err != nil {
		return fmt.Errorf("Can't find id to delete")
	}
	return nil
}

func (service *EncounterService) Update(encounter *model.Encounter) (*model.Encounter, error) {
	if err := service.EncounterRepo.Update(encounter); err == nil {
		return encounter, nil
	}

	return nil, fmt.Errorf("Couldn't approve.")
}
func (service *EncounterService) Approve(encounter *model.Encounter) (*model.Encounter, error) {

	encounter.ApprovalStatus = model.EncounterApprovalStatus("ADMIN_APPROVED")
	if err := service.EncounterRepo.Update(encounter); err == nil {
		return encounter, nil
	}

	return nil, fmt.Errorf("Couldn't approve.")

}

func (service *EncounterService) Decline(encounter *model.Encounter) (*model.Encounter, error) {

	encounter.ApprovalStatus = model.EncounterApprovalStatus("DECLINED")
	if err := service.EncounterRepo.Update(encounter); err == nil {
		return encounter, nil
	}

	return nil, fmt.Errorf("Couldn't approve.")
}

func findInVicinity(encountersPtr *[]*model.Encounter, lat, long float64) []*model.Encounter {
	encounters := *encountersPtr

	var results []*model.Encounter
	for _, e := range encounters {
		if distance := CalculateDistance(e.Latitude, e.Longitude, lat, long); distance <= e.Range {
			fmt.Println(distance, e.Range)
			results = append(results, e)
		}
	}
	return results
}

func getLatLong(userId int) (float64, float64, error) {
	url := "http://localhost:8000/positions/getByUser/" + strconv.Itoa(userId)

	response, err := http.Get(url)
	if err != nil {
		return 0, 0, err
	}
	defer response.Body.Close()

	body, err := io.ReadAll(response.Body)
	if err != nil {
		return 0, 0, err
	}

	var data map[string]interface{}
	if err := json.Unmarshal(body, &data); err != nil {
		return 0, 0, err
	}

	long, ok1 := data["Longitude"].(float64)
	lat, ok2 := data["Latitude"].(float64)

	if !ok1 || !ok2 {
		return 0, 0, err
	}

	return lat, long, nil
}
