import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Profile } from '../model/profile.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ProfileService } from '../profile.service';
import { ActivatedRoute } from '@angular/router';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'xp-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  showContent = 'showProfile';
  updateProfile = false;
  profilesToFollow: Profile[] = [];
  personUpdateForm: FormGroup;
  //showProfileSection: boolean = true;  // Initially show profile section
  //showEditProfileSection: boolean = true;  // Initially show edit profile section
  showForm: boolean = false;
  
  editActive: boolean = true;
  chatActive: boolean = false;
  followingActive: boolean = false;
  followersActive: boolean = false;
  peopleActive: boolean = false;

  followers: Profile[];
  following: Profile[];

  profile: Profile = {
    name: '',
    surname: '',
    profileImage: '',
    biography: '',
    quote: '',
    username: '',
    password: '',
    email: '',
    userId: 0,
    xp: 0,
    level: 0
  }
  constructor(private cd: ChangeDetectorRef, private service: ProfileService, private auth: AuthService, private formBuilder: FormBuilder) {
    this.personUpdateForm = this.formBuilder.group({
      newName: new FormControl('', Validators.required),
      newSurname: new FormControl('', Validators.required),
      newProfileImage: new FormControl('', Validators.required),
      newBiography: new FormControl('', Validators.required),
      newQuote: new FormControl('', Validators.required),
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      userId: 0
    });
  }

  ngOnInit(): void {
    this.loadProfileData();
    this.loadProfileFollowers();
    this.loadProfileFollowing();
    this.getRecommendedProfiles();
  }

  loadProfileData() {
    this.auth.user$.subscribe((user) => {
      if (user.username) {
        const userId = user.id;
        this.service.getProfile(userId).subscribe({
          next: (data: Profile) => {
            this.profile.name = data.name;
            this.profile.surname = data.surname;
            this.profile.profileImage = data.profileImage;
            this.profile.biography = data.biography;
            this.profile.quote = data.quote;
            this.profile.xp = data.xp;
            this.profile.level = data.level;
          },
          error: (err: any) => {
            console.log(err);
          }
        });
      }
    });
  }

  toggleForm() {
    this.showForm = !this.showForm;
  }

  loadProfileFollowers() {
    this.service.getFollowers().subscribe({
      next: (data: PagedResults<Profile>) => {
        this.followers = data.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  loadProfileFollowing() {
    this.service.getFollowing().subscribe({
      next: (data: PagedResults<Profile>) => {
        this.following = data.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  getRecommendedProfiles(): void {
    this.service.getRecommendedProfiles().subscribe({
      next: (data: PagedResults<Profile>) => {
        this.profilesToFollow = data.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  
  activeSection: string = 'showEdit';

  showTable(show:string) :void {
    this.activeSection = show;
  }
  

  unfollow(followingId: number) {
    this.service.unfollow(followingId).subscribe({
      next: (data: PagedResults<Profile>) => {
        this.loadProfileFollowing();
        this.getRecommendedProfiles();
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  follow(followingId: number) {
    this.service.follow(followingId).subscribe({
      next: (data: PagedResults<Profile>) => {
        this.loadProfileFollowing();  
        this.getRecommendedProfiles();  
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  onSubmit() {
    if (this.personUpdateForm.valid) {
      this.auth.user$.subscribe((user) => {
        if (user.username) {
          const userId = user.id;
          const updatedProfile: Profile = {
            name: this.personUpdateForm.value.newName,
            surname: this.personUpdateForm.value.newSurname,
            profileImage: this.personUpdateForm.value.newProfileImage,
            biography: this.personUpdateForm.value.newBiography,
            quote: this.personUpdateForm.value.newQuote,
            username: this.personUpdateForm.value.username,
            password: this.personUpdateForm.value.password,
            email: this.personUpdateForm.value.email,
            userId: user.id,
            xp: this.profile.xp,
            level: this.profile.level
          };
          this.service.updateProfile(userId, updatedProfile).subscribe({
            next: (data: Profile) => {
              this.personUpdateForm.reset();
              this.loadProfileData();
              this.cd.detectChanges();
            },
            error: (err: any) => {
              console.log(err);
            }
          });
        }
      });
    }
  }

  showUpdateForm() : void {
    this.updateProfile = true;
  }

  cancelUpdate() : void {
    this.updateProfile = false;
  }
}
