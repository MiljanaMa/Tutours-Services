import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Bundle } from '../model/bundle.model';
import { MarketplaceService } from '../marketplace.service';

@Component({
  selector: 'xp-bundle-form',
  templateUrl: './bundle-form.component.html',
  styleUrls: ['./bundle-form.component.css']
})
export class BundleFormComponent implements OnInit  {
  form: FormGroup;

  constructor(private fb: FormBuilder, private service: MarketplaceService) { }

  ngOnInit(): void {
    // Initialize the form with default values or leave empty if not needed
    this.form = this.fb.group({
      Name: ['', Validators.required],
      
      Status: ['', Validators.required],
      id: [0, Validators.required],
      
    });
  }

  // Function to handle form submission
  onSubmit() {
    if (this.form.valid) {
      const formData: Bundle = this.form.value;
      formData.tours = []
      // Call the service method to create the bundle
      this.service.createBundle(formData).subscribe(
        (response:Bundle) => {
          console.log('Bundle created successfully:', response);
          // Add any additional logic here, such as navigation or displaying a success message
        },
        (error) => {
          console.error('Error creating bundle:', error);
          // Handle the error, e.g., display an error message to the user
        }
      );
    }
  }
}
