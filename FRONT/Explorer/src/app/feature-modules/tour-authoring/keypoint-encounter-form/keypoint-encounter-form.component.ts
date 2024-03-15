import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Encounter, EncounterApprovalStatus, KeypointEncounter } from '../model/keypointEncounter.model';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { TourAuthoringService } from '../tour-authoring.service';
import { Keypoint } from '../model/keypoint.model';

@Component({
  selector: 'xp-keypoint-encounter-form',
  templateUrl: './keypoint-encounter-form.component.html',
  styleUrls: ['./keypoint-encounter-form.component.css']
})
export class KeypointEncounterFormComponent {
  @Output() encounterUpdated = new EventEmitter<KeypointEncounter>();
  
  @Input() mode: string = 'add';
  @Input() selectedEncounter: KeypointEncounter;
  @Input() keypoint: Keypoint;

  public encounterForm: FormGroup;

  constructor(private tourAuthoringService: TourAuthoringService) {
    this.encounterForm = new FormGroup ({
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      xp: new FormControl(0, [Validators.min(1)]),
      status: new FormControl(''), 
      type: new FormControl('', [Validators.required]),
      range: new FormControl('', [Validators.min(1)]),
      image: new FormControl(null),
      peopleCount: new FormControl(0),
      isRequired: new FormControl(''),
    });

    this.encounterForm.controls["peopleCount"].addValidators([this.customPeopleValidator()]);
    this.encounterForm.controls["image"].addValidators([this.customImageValidator()]);
    this.encounterForm.controls["status"].addValidators([this.customStatusValidator()]);
  }

  ngOnChanges(): void {
    this.encounterForm.reset();
    if(this.mode === 'edit') {
      let encounterValues = {
        name: this.selectedEncounter.encounter.name || "",
        description: this.selectedEncounter.encounter.description || "",
        xp: this.selectedEncounter.encounter.xp || 10,
        status: this.selectedEncounter.encounter.status || 'DRAFT',
        type: this.selectedEncounter.encounter.type || 'SOCIAL',
        range: this.selectedEncounter.encounter.range || 0,
        image: this.selectedEncounter.encounter.image,
        peopleCount: this.selectedEncounter.encounter.peopleCount,
        keypointId: this.selectedEncounter.id,
        encounterId: this.selectedEncounter.encounter.id,
        isRequired: (this.selectedEncounter.isRequired === true ? 'YES': 'NO'),
      };
      this.encounterForm.patchValue(encounterValues);
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
    let formEncounter: Encounter = {
      id: (this.mode === 'add' ? 0 : (this.selectedEncounter.encounterId || 0)),
      name: this.encounterForm.value.name || "",
      description: this.encounterForm.value.description || "",
      latitude: this.keypoint.latitude,
      longitude: this.keypoint.longitude,
      xp: this.encounterForm.value.xp || 10,
      status: this.encounterForm.value.status || 'DRAFT',
      type: this.encounterForm.value.type || 'SOCIAL',
      range: this.encounterForm.value.range || 0,
      image: this.encounterForm.value.image,
      peopleCount: this.encounterForm.value.peopleCount,
      approvalStatus: EncounterApprovalStatus.PENDING
    };
    let keypointEncounter: KeypointEncounter = {
      encounter: formEncounter,
      keypointId: this.keypoint.id || 0,
      encounterId: (this.mode === 'add' ? 0 : (this.selectedEncounter.encounterId|| 0)),
      isRequired: (this.encounterForm.value.isRequired === 'YES' ? true : false),
    };
    
    if(this.mode === 'add'){
      this.tourAuthoringService.addEncounter(keypointEncounter).subscribe({
        next: () => { 
          this.encounterUpdated.emit();
          this.encounterForm.reset();
        }
      });
    }else if( this.mode === 'edit'){
      keypointEncounter.id = this.selectedEncounter.id;
      keypointEncounter.encounter.userId = this.selectedEncounter.encounter.userId;
      keypointEncounter.encounter.approvalStatus = this.selectedEncounter.encounter.approvalStatus;
      this.tourAuthoringService.updateEncounter(keypointEncounter).subscribe({
        next: () => {
          this.encounterUpdated.emit(); 
          this.encounterForm.reset();
        }
      });
    }
  }

}
