import { Component, OnDestroy, OnInit } from '@angular/core';
import { TourExecutionStatus, TourProgress } from '../model/tour-progress.model';
import { TourExecutionService } from '../tour-execution.service';
import { RouteQuery } from 'src/app/shared/model/routeQuery.model';
import { MarkerPosition } from 'src/app/shared/model/markerPosition.model';
import { Subscription, interval, Subject, Observable, of } from 'rxjs';
import { Keypoint } from '../../tour-authoring/model/keypoint.model';
import { Encounter, EncounterStatus, EncounterType, KeypointEncounter } from '../../tour-authoring/model/keypointEncounter.model';
import { TourAuthoringService } from '../../tour-authoring/tour-authoring.service';
import { switchMap, takeUntil } from 'rxjs/operators';
import { EncountersService } from '../../encounters-managing/encounters.service';
import { PagedResult } from '../shared/model/paged-result.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { EncounterCompletion, EncounterCompletionStatus } from '../../encounters-managing/model/encounterCompletion.model';
import { Blog, BlogSystemStatus } from '../../blog/model/blog.model';
import { TouristPosition } from '../model/tourist-position.model';
import { MarketplaceService } from '../../marketplace/marketplace.service';
import { Category, Object } from '../../tour-authoring/model/object.model';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

enum PointOfInterestType {
  publicObjects = 1,
  activeEncounters,
  completedEncounters,
  notStartedEncounters,
  publicKeypoints,
  position,
  activeTour,
}

@Component({
  selector: 'xp-active-tour',
  templateUrl: './active-tour.component.html',
  styleUrls: ['./active-tour.component.css'],
  providers: [MessageService]
})
export class ActiveTourComponent implements OnInit, OnDestroy {
  activeTour: TourProgress | undefined;
  touristPosition: TouristPosition | undefined;
  routeQuery: RouteQuery | undefined;
  currentPosition: MarkerPosition | undefined;
  
  refreshMap: boolean = false;
  currentKeyPoint: Keypoint | undefined;

  public keypointEncounters: KeypointEncounter[];
  public requiredEncounters: KeypointEncounter[] = [];

  showBlog: boolean = false;
  activeTourCopy : TourProgress | undefined;
  newBlog: Blog = {
    title: '',
    description: '',
    imageLinks: [],
    creationDate: Date.now().toString(),
    systemStatus: BlogSystemStatus.DRAFT
  };

  public positionSetMode: boolean;

  public pointsOfInterest: MarkerPosition[];
  public nearbyEncounters: Encounter[];
  // saving all points of interest separately so they can easily be hidden from the map
  private completedEncounters: MarkerPosition[];
  private activeEncounters: MarkerPosition[];
  private notStartedEncounters: MarkerPosition[];
  private publicObjects: MarkerPosition[];
  private publicKeypoints: MarkerPosition[];

  private temporary: MarkerPosition[];

  private updateSubscription: Subscription | undefined;
  private destroy$ = new Subject<void>();

  public nearbyObjectsToShow: PointOfInterestType[];
  public get PointOfInterestType() {
    return PointOfInterestType; 
  }
  public get EncounterType() {
    return EncounterType; 
  }

  public activeTab: string;
  public openImagePopup: boolean;

  public mode: string = 'edit'; // add or edit (copied from tourist-position window)

  constructor(private service: TourExecutionService, private tourAuthoringService: TourAuthoringService, private encounterService: EncountersService, private marketplaceService: MarketplaceService, private messageService: MessageService) { }

  ngOnInit(): void {
    this.getActiveTour();
    this.updatePosition();
    this.refreshMap = true;
    this.positionSetMode = true;
    this.openImagePopup = false;
    this.activeTab = 'activeTour';
    this.nearbyObjectsToShow = [ 
      PointOfInterestType.publicObjects ,
      PointOfInterestType.activeEncounters,
      PointOfInterestType.completedEncounters,
      PointOfInterestType.notStartedEncounters,
      PointOfInterestType.publicKeypoints,
      PointOfInterestType.position,
      PointOfInterestType.activeTour
    ];

    setTimeout(() => {
      this.checkNearbyEncounters();
      this.getNearbyEncounters();
      this.getNearbyObjects();
      this.getNearbyKeypoints();
    }, 1000);

    setTimeout(() => {
      this.loadPointsOfInterest(); 
      this.loadMapInfo();
    }, 6000);
    // this.updateSubscription = interval(10000).pipe(
    //   switchMap(() => this.activeTour !== undefined ? this.getKeypointActiveEncounters() : []),
    //   takeUntil(this.destroy$)
    // ).subscribe(() => {
    //   if (this.activeTour !== undefined) {
    //     this.updatePosition();
    //     this.checkNearbyEncounters();
    //     this.getNearbyEncounters();
    //     this.getNearbyObjects();
    //     this.getNearbyKeypoints();

    //     // temporary, will be changed after currentPosition update:
    //     setTimeout(() => {
    //       this.loadPointsOfInterest(); 
    //       this.loadMapInfo();
    //     }, 2000);
    //   }
    // });
  }

  triggerMapRefresh(): void {
    this.refreshMap = false; 
    setTimeout(() => {
      this.refreshMap = true; 
    });
  }

  startEncounter(encounter: Encounter): void {
    this.encounterService.startEncounter(encounter).subscribe({
      next: (result: EncounterCompletion) =>{
        this.messageService.add({severity: 'success', summary: 'Success', detail: 'Encounter started.'});
        this.getNearbyEncounters(true);
        setTimeout(() => {
          this.checkNearbyEncounters(true);
        }, 15000);
        setTimeout(() => {
          this.checkNearbyEncounters(true);
        }, 30000);
      },
      error: (error) => {
        alert("Cannot start encounter");
      }
    });
  }

  loadPointsOfInterest() {
    this.temporary = [];

    this.nearbyObjectsToShow.forEach(pointsOfInterestType => {
      switch(pointsOfInterestType){
        case PointOfInterestType.activeEncounters:
          this.temporary.push(...this.activeEncounters);
          break;
        case PointOfInterestType.completedEncounters:
          this.temporary.push(...this.completedEncounters);
          break;
        case PointOfInterestType.notStartedEncounters:
          this.temporary.push(...this.notStartedEncounters);
          break;
        case PointOfInterestType.publicObjects:
          this.temporary.push(...this.publicObjects);
          break;
        case PointOfInterestType.publicKeypoints:
          this.temporary.push(...this.publicKeypoints);
          break;
      }
    });

    this.pointsOfInterest = this.temporary; // map refresh
  }

  loadMapInfo() {
    if(this.nearbyObjectsToShow.indexOf(PointOfInterestType.activeTour) > -1 && this.activeTour) {
      this.routeQuery = {
        keypoints: this.activeTour.tour.keypoints || [],
        transportType: this.activeTour.tour.transportType || 'WALK'
      }
    }
    else {
      this.routeQuery = undefined;
      this.triggerMapRefresh();
    }

    if(this.nearbyObjectsToShow.indexOf(PointOfInterestType.position) > -1 && this.activeTour) {
      this.currentPosition = {
        longitude: this.touristPosition?.longitude || 0,
        latitude: this.touristPosition?.latitude || 0,
        color: 'walking'
      }
    }
    else {
      this.currentPosition = undefined;
    }
  }

  getNearbyObjects() {
    this.publicObjects = [];

    this.marketplaceService.getPublicObjects(0, 0, this.currentPosition?.latitude || 0, this.currentPosition?.longitude || 0, 1).subscribe({
      next: (result: PagedResult<Object>) => {
        result.results.forEach((obj) => {
          this.publicObjects.push({
            longitude: obj.longitude,
            latitude: obj.latitude,
            color: obj.category == Category.OTHER ? 'object' : obj.category.toString().toLowerCase(),
            title: obj.name
          })
        });
      }
    })
  }

  getNearbyKeypoints() {
    this.publicKeypoints = [];

    this.marketplaceService.getPublicKeyPoints(0, 0, this.currentPosition?.latitude || 0, this.currentPosition?.longitude || 0, 1).subscribe({
      next: (result: PagedResult<Keypoint>) => {
        result.results.forEach((kp) => {
          this.publicKeypoints.push({
            longitude: kp.longitude,
            latitude: kp.latitude,
            color: 'red',
            title: kp.name
          })
        });
      }
    })
  }

  getNearbyEncounters(loadPOI: boolean = false): void{
    this.activeEncounters = [];
    this.completedEncounters = [];
    this.notStartedEncounters = [];

    this.encounterService.getNearbyEncounters().subscribe({
      next: (encountersResult: PagedResults<Encounter>) => {
        this.nearbyEncounters = encountersResult.results;
        var encounterIds = encountersResult.results.map((enc) => enc.id);
        if(encounterIds != undefined){
          this.encounterService.getEncounterCompletionsByIds(encounterIds).subscribe({
            next: (result: EncounterCompletion[]) => {
              encountersResult.results.forEach(encounter => {
                console.log(result);
                var encounterCompletions = (result && result.length > 0 && result[0] != null) ? result.filter(ec => ec.encounterId === encounter.id) : null;
                var encounterColor = encounter.type.toString().toLowerCase(), encounterRange = 0;
                if(encounterCompletions != null && encounterCompletions.length > 0) {
                  var encounterCompletion = encounterCompletions[0];
                  switch(encounterCompletion.status){
                    case EncounterCompletionStatus.PROGRESSING:
                    case EncounterCompletionStatus.STARTED:
                      encounterColor = encounterColor + "-started";
                      encounterRange = encounter.range;
                      this.activeEncounters.push({
                        longitude: encounter.longitude,
                        latitude: encounter.latitude,
                        color: encounterColor,
                        title: encounter.name,
                        radiusSize: encounterRange
                      })
                      break;
                    case EncounterCompletionStatus.COMPLETED:
                      encounterColor = encounterColor + "-completed";
                      this.completedEncounters.push({
                        longitude: encounter.longitude,
                        latitude: encounter.latitude,
                        color: encounterColor,
                        title: encounter.name,
                        radiusSize: encounterRange
                      })
                      break;
                  }
                }
                else {
                  this.notStartedEncounters.push({
                    longitude: encounter.longitude,
                    latitude: encounter.latitude,
                    color: encounterColor,
                    title: encounter.name,
                    radiusSize: encounterRange
                  })
                }
              });

              if(loadPOI) {
                this.loadPointsOfInterest();
              }
            }
          })
        }
      }
    });
  }

  // dummy way for updating nearby stuff, can't bother now to do it better on backend
  checkNearbyEncounters(loadPOI: boolean = false): void {
    this.encounterService.checkNearbyEncounters().subscribe({
      next: (result: PagedResults<EncounterCompletion>) => {
        if(result.results){
          result.results.forEach((encounterCompletion) => {
            alert('WOOO! You completed an encounter' /*+ encounterCompletion.encounter.name*/); // nj
            console.log(loadPOI);
          });
        }
        if(loadPOI) {
          this.getNearbyEncounters(true);
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();

    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
  }

  updatePosition(): void {
    if (this.requiredEncounters.length !== 0) {
      // Do something with requiredEncounters if needed
      this.service.getTouristPosition().subscribe({
          next: (result: TouristPosition) =>{
            if(this.currentPosition?.longitude !== result.longitude || this.currentPosition.latitude !== result.latitude){
              this.touristPosition = result;
            }
          }
      });
      return;
    }

    this.service.updateActiveTour((this.requiredEncounters.length === 0)).pipe(
      switchMap((result: TourProgress) => {
        if(!result.touristPosition) return of(null);
        this.currentPosition = result.touristPosition;
        this.currentPosition.color = "walking";
        let previousSecret = this.currentKeyPoint?.secret;
        let previousKeypoint = this.currentKeyPoint;

        if (result.status === 'COMPLETED') {
          this.activeTour = undefined;
          this.routeQuery = undefined;
          this.currentPosition = undefined;
          this.refreshMap = false;
          this.currentKeyPoint = undefined;
          //window.confirm(previousSecret)
          //this.showBlogForm(result.status);
          this.activeTab = 'encounters';
          this.messageService.add({severity:'success', summary:'Success', detail:'Tour completed.'})
          return of(null); // Return an observable to continue the chain
        }

        if (this.activeTour && this.activeTour.status === 'IN_PROGRESS') {
          this.currentKeyPoint = this.activeTour.tour.keypoints?.find((keypoint) => keypoint.position === result.currentKeyPoint);
          if (previousKeypoint != this.currentKeyPoint && this.currentKeyPoint?.secret !== "") {
            // if (window.confirm(previousSecret)) {
            //   this.triggerMapRefresh();
            // }
          }
          return this.getKeypointActiveEncounters();
        } else {
          this.triggerMapRefresh();
          return of(null);
        }
      }),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      //this.triggerMapRefresh();
    });
  }

  getKeypointActiveEncounters(): Observable<any> {
    return this.tourAuthoringService.getKeypointEncounters(this.currentKeyPoint?.id || 0).pipe(
      switchMap((result: PagedResults<KeypointEncounter>) => {
        this.keypointEncounters = (result.results).filter((item: any) => item.encounter.status === EncounterStatus.ACTIVE);
        return this.getTouristCompletedEncounters();
      }),
      takeUntil(this.destroy$)
    );
  }

  getTouristCompletedEncounters(): Observable<any> {
    return this.service.getTouristCompletedEncounters().pipe(
      switchMap((result: PagedResults<EncounterCompletion>) => {
        let completedEncounters = (result.results).filter(item => item.status === 'COMPLETED');
        const completedIds = completedEncounters.map(r => r.encounter.id);
        this.requiredEncounters = this.keypointEncounters.filter(item => !completedIds.includes(item.encounterId))
        this.requiredEncounters = this.requiredEncounters.filter(item => item.isRequired === true);
        return of(null);
      }),
      takeUntil(this.destroy$)
    );
  }

  getActiveTour(): void {
    this.service.getActiveTour().subscribe({
      next: (result: TourProgress) => {
        this.activeTour = result;
        this.activeTourCopy = result;

        this.touristPosition = this.activeTour.touristPosition;

        if (this.activeTour && this.activeTour.status === 'IN_PROGRESS') {
          this.currentKeyPoint = this.activeTour.tour.keypoints?.find((keypoint) => keypoint.position === this.activeTour?.currentKeyPoint);
        }
        this.getKeypointActiveEncounters();
      },
      error: () => { 
        this.getPosition();
      }
    })
  }

  abandonTour(): void {
    if (!window.confirm('Are you sure you want to abandon this tour?')) {
      return;
    }

    this.service.abandonTour().subscribe({
      next: (result: TourProgress) => {
        this.activeTour = undefined;
        this.routeQuery = undefined;
        this.currentPosition = undefined;
      }
    })
  }

  showBlogForm(status: TourExecutionStatus): void{
    if(status === 'COMPLETED')
    {            
      this.showBlog = confirm("Would you like create a blog?");
    }
  }
 
  toggleSetting(setting: PointOfInterestType) {
    const settingIndex = this.nearbyObjectsToShow.indexOf(setting);
    if(settingIndex > -1) {
      this.nearbyObjectsToShow.splice(settingIndex, 1);
    }
    else {
      this.nearbyObjectsToShow.push(setting);
    }

    this.loadPointsOfInterest(); 
    this.loadMapInfo();
  }

  changePosition(): void {
    this.positionSetMode = true;
    // this.temporary = this.pointsOfInterest;
    // this.pointsOfInterest = [];
    console.log(this.positionSetMode);

  }

  getPosition(): void {
    this.service.getTouristPosition().subscribe({
      next: (result: TouristPosition) => { 
        this.touristPosition = result;
        this.currentPosition = {
          latitude: this.touristPosition.latitude,
          longitude: this.touristPosition.longitude
        }
        this.mode = 'edit';
      },
      error: () => { 
        this.mode = 'add';
      }
    });
  }

  updateTouristPosition(event: number[]): void {
    this.touristPosition = {
      latitude: event[0],
      longitude: event[1]
    }
    
    this.confirmPosition();
  }

  confirmPosition(): void {
    if(this.touristPosition == null) {
      window.alert("Please select your position");
      return;
    }

    if (this.mode === 'edit' ) {
      this.service.updateTouristPosition(this.touristPosition).subscribe({
        next: () => {

          this.checkNearbyEncounters();
          this.getNearbyEncounters();
          this.getNearbyObjects();
          this.getNearbyKeypoints();
          this.updatePosition();

          // sadsadkhasudhasu
          setTimeout(() => {
            this.loadPointsOfInterest(); 
            this.loadMapInfo();
          }, 1000);

          this.service.updateSocialEncounters().subscribe({
            next: () => {
          }
          });
        },
      });
    }
    else if (this.mode ==='add') {
      this.service.addTouristPosition(this.touristPosition).subscribe({
        next: () => {
          window.alert("Position successfully added");  
          this.mode = 'edit';

          this.service.updateSocialEncounters().subscribe({
            next: () => {
          }
          });
          
        },
      });
    }
  }

  openImage(): void {
    this.openImagePopup = true;
  }
}
