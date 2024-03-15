import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { LayoutService } from '../layout.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { NotificationModel } from '../model/notification.model';
import { Router } from '@angular/router';
import {ProfileService} from "../../administration/profile.service";
import {Profile} from "../../administration/model/profile.model";

@Component({
  selector: 'xp-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  @ViewChild('overlayContainer',  { static: false })  overlayContainer: ElementRef | undefined;
  user: User | undefined;
  showOldToolbar: boolean = false;
  notifications: NotificationModel[] = [];
  unreadNotificationsCount: number = 0;
  isOverlayOpen: boolean = false;

  profile: Profile = {} as Profile;

  constructor(private authService: AuthService, private layoutService: LayoutService, private router: Router, private service: ProfileService) {}

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.getNotifications();
      this.loadProfileData();
    });
  }

  @HostListener('document:click', ['$event'])
  handleClick(event: Event) {
    if (this.overlayContainer && !this.overlayContainer.nativeElement.contains(event.target)) {
      this.closeOverlay();
    }
  }

  toggleMenu(event: Event) {
    event.stopPropagation();
    this.isOverlayOpen = !this.isOverlayOpen;
  }

  closeOverlay() {
    this.isOverlayOpen = false;
  }

  onLogout(): void {
    this.authService.logout();
  }

  toggleOldToolbar(): void {
    this.showOldToolbar = !this.showOldToolbar;
  }

  getNotifications(): void {
    this.layoutService.getNotifications().subscribe({
      next: (result: PagedResults<NotificationModel>) => {
        this.notifications = result.results;
        this.unreadNotificationsCount = this.notifications.filter(n => !n.isRead).length;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  markAsRead(notification: NotificationModel): void {
    this.layoutService.markAsRead(notification).subscribe({
      next: () => {
        this.getNotifications();
        this.unreadNotificationsCount -= 1;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  delete(notificationId: number): void {
    this.layoutService.delete(notificationId).subscribe({
      next: () => {
        this.getNotifications();
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  redirect(actionURL: string): void {
    if(actionURL){
      this.router.navigate([actionURL]);
    }
  }

  loadProfileData() {
    this.authService.user$.subscribe((user) => {
      if (user.username) {
        const userId = user.id;
        this.service.getProfile(userId).subscribe({
          next: (data: Profile) => {
            this.profile = data;
          },
          error: (err: any) => {
            console.log(err);
          }
        });
      }
    });
  }

  protected readonly localStorage = localStorage;
}
