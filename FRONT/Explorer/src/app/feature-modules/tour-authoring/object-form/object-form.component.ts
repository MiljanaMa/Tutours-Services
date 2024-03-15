import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TourAuthoringService } from '../tour-authoring.service';
import { Object } from '../model/object.model';

@Component({
  selector: 'xp-object-form',
  templateUrl: './object-form.component.html',
  styleUrls: ['./object-form.component.css']
})
export class ObjectFormComponent implements OnChanges{

  @Output() objectUpdated = new EventEmitter<null>();
  @Input() object: Object;
  @Input() mode: string = 'add';

  public objectForm: FormGroup;

  constructor(private tourAuthoringService: TourAuthoringService) {
    this.objectForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      image: new FormControl(''),
      category: new FormControl(''),
      latitude: new FormControl(0, [Validators.min(-90), Validators.max(90)]),
      longitude: new FormControl(0, [Validators.min(-180), Validators.max(180)])
    });
  }

  ngOnChanges(): void {
    this.objectForm.reset();
    if(this.mode === 'edit') {
      this.objectForm.patchValue(this.object);
    }
  }

  saveObject(): void {
    let object: Object = {
      id: 0,
      name: this.objectForm.value.name || "",
      description: this.objectForm.value.description || "",
      latitude: this.objectForm.value.latitude || "",
      longitude: this.objectForm.value.longitude || "",
      image: this.objectForm.value.image || "",
      category: this.objectForm.value.category || "",
      status: 0
    };
    if(this.mode === 'add'){
      this.tourAuthoringService.addObject(object).subscribe({
        next: () => { this.objectUpdated.emit() }
      });
    }else if( this.mode === 'edit'){
      object.id = this.object.id;
      this.tourAuthoringService.updateObject(object).subscribe({
        next: () => { this.objectUpdated.emit() }
      });
    } 
  }
}
