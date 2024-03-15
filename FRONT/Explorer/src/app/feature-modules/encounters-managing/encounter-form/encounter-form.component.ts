import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { Encounter, EncounterApprovalStatus } from '../model/encounter.model';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { EncountersService } from '../encounters.service';

@Component({
  selector: 'xp-encounter-form',
  templateUrl: './encounter-form.component.html',
  styleUrls: ['./encounter-form.component.css']
})
export class EncounterFormComponent implements OnChanges {
  @Output() encounterUpdated = new EventEmitter<Encounter>();
  
  @Input() mode: string = 'add';
  @Input() selectedEncounter: Encounter;

  public mapControl: string;

  public encounterForm: FormGroup;

  constructor(private encounterService: EncountersService) {
    this.encounterForm = new FormGroup ({
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      latitude: new FormControl(0, [Validators.min(-90), Validators.max(90)]),
      longitude: new FormControl(0, [Validators.min(-180), Validators.max(180)]),
      xp: new FormControl(0, [Validators.min(1)]),
      status: new FormControl(''), 
      type: new FormControl('', [Validators.required]),
      range: new FormControl('', [Validators.min(1)]),
      image: new FormControl(null),
      imageLatitude: new FormControl(null),
      imageLongitude: new FormControl(null),
      peopleCount: new FormControl(0),
    });

    this.encounterForm.controls["peopleCount"].addValidators([this.customPeopleValidator()]);
    this.encounterForm.controls["image"].addValidators([this.customImageValidator()]);
    this.encounterForm.controls["status"].addValidators([this.customStatusValidator()]);

    this.mapControl = 'encounterLocation';
  }

  ngOnChanges(): void {
    this.encounterForm.reset();
    if(this.mode === 'edit') {
      this.encounterForm.patchValue(this.selectedEncounter);
    }
  }
  customPeopleValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const type = this.encounterForm.get('type')?.value;
      const peopleCount = this.encounterForm.get('peopleCount')?.value;

      if (type === 'SOCIAL' && (peopleCount === null || peopleCount < 1)) {
        return { invalidPeopleCount: true, message: 'For SOCIAL type, at least 1 person is required.' };
      }
      return null;
    };
  }
  customImageValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const type = this.encounterForm.get('type')?.value;
      const image = this.encounterForm.get('image')?.value;

      if (type === 'LOCATION' && (image === null || image === '')) {
        return { invalidImage: true, message: 'Image is required.' };
      }
      return null;
    };
  }
  customStatusValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const status = this.encounterForm.get('status')?.value;

      if (this.mode === 'edit' && status === null) {
        return { invalidStatus: true, message: 'Status is required.' };
      }
      return null;
    };
  }

  saveEncounter(): void {
    let encounter: Encounter = {
      name: this.encounterForm.value.name || "",
      description: this.encounterForm.value.description || "",
      latitude: this.encounterForm.value.latitude || 0,
      longitude: this.encounterForm.value.longitude || 0,
      xp: this.encounterForm.value.xp || 10,
      status: this.encounterForm.value.status || 'DRAFT', 
      type: this.encounterForm.value.type || 'SOCIAL',
      range: this.encounterForm.value.range || 0,
      image: this.encounterForm.value.image,
      imageLatitude: this.encounterForm.value.imageLatitude,
      imageLongitude: this.encounterForm.value.imageLongitude,
      peopleCount: this.encounterForm.value.peopleCount,
      approvalStatus: EncounterApprovalStatus.PENDING
    };
    
    if(this.mode === 'add'){
      this.encounterService.addEncounter(encounter).subscribe({
        next: () => { 
          this.encounterUpdated.emit();
          this.encounterForm.reset();
        }
        ,error: () => {
        }
      });
    }else if( this.mode === 'edit'){
      encounter.id = this.selectedEncounter.id;
      encounter.userId = this.selectedEncounter.userId;
      encounter.approvalStatus = this.selectedEncounter.approvalStatus;
      this.encounterService.updateEncounter(encounter).subscribe({
        next: () => {
          this.encounterUpdated.emit(); 
          this.encounterForm.reset();
        }
      });
    } 
  }

  fillCoords(event: number[]): void {
    if(this.mapControl ==  'encounterLocation'){
      this.encounterForm.patchValue({
        latitude: event[0],
        longitude: event[1]
      })
    }
    else if(this.mapControl == 'imageLocation'){
      this.encounterForm.patchValue({
        imageLatitude: event[0],
        imageLongitude: event[1]
      })
    }
  }

  toggleMapControl(): void {
    this.mapControl == 'encounterLocation' ? this.mapControl = 'imageLocation' : this.mapControl = 'encounterLocation';
  }
}
