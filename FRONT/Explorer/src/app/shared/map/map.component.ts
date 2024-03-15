import { Component, AfterViewInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import * as L from 'leaflet';
import 'leaflet-routing-machine';
import { MapService } from '../map.service';
import { environment } from 'src/env/environment';
import { TestTour } from '../model/testtour.model';
import { CommonModule } from '@angular/common';
import { Keypoint } from 'src/app/feature-modules/tour-authoring/model/keypoint.model';
import { RouteQuery } from '../model/routeQuery.model';
import { RouteInfo } from '../model/routeInfo.model';
import { TransportType } from 'src/app/feature-modules/tour-authoring/model/tour.model';
import { MarkerPosition } from '../model/markerPosition.model';

@Component({
  standalone: true,
  selector: 'app-map',
  imports: [CommonModule],
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css'],
})
export class MapComponent implements AfterViewInit, OnChanges {
  private readonly markersPath = "assets/map-markers";

  private map: any;
  private routeControl: L.Routing.Control;
  private clickMarker: L.Marker; // saving just so we can access current lng/lat when needed
  private markerLayer: L.LayerGroup; // layer that contains all markers (except markers that are part of the route)
  private drawLayer: L.LayerGroup; // layer that contains all drawings (radiuses)

  @Output() clickEvent = new EventEmitter<number[]>();
  @Output() routesFoundEvent = new EventEmitter<RouteInfo>();

  @Input() selectedTour: TestTour; // ??
  @Input() markType: string; // ??

  @Input() routeQuery: RouteQuery | undefined;
  @Input() markerPosition: MarkerPosition | undefined; // one marker that will be shown, separated from the rest for displaying current location or similar
  @Input() markerPositions: MarkerPosition[]; // list of markers that will be shown, use 'color' property to change marker Icon look
  @Input() radiusSize: number; // radius that's drawn around the marker that appears on click

  @Input() enableClicks: boolean;
  @Input() toggleOff: boolean;
  @Input() allowMultipleMarkers: boolean;
  @Input() moveMarkers: boolean;
  @Input() drawRadiusOnClick: boolean;
  @Input() fitSelectedRoutes: boolean; // when 'true', map will always focus found route to be on the center
  @Input() rememberClick: boolean; // when 'true' the current marker (the one that appears on click) will be shown even after the map refresh. It will only disappear in case user clicks again on the map

  //#region Icons
  customIconSize: L.PointExpression = [48, 48];
  customIconAnchor: L.PointExpression = [24, 46];

  // there is probably an easier way for this
  oldBlueIcon = L.icon({
    iconUrl: 'https://unpkg.com/leaflet@1.6.0/dist/images/marker-icon.png',
  });
  yellowIcon = L.icon({
    iconUrl: `${this.markersPath}/yellow-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  redIcon = L.icon({
    iconUrl: `${this.markersPath}/red-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  greenIcon = L.icon({
    iconUrl: `${this.markersPath}/green-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  blueIcon = L.icon({
    iconUrl: `${this.markersPath}/blue-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });

  gamblingIcon = L.icon({
    iconUrl: `${this.markersPath}/gambling-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  gamblingStartedIcon = L.icon({
    iconUrl: `${this.markersPath}/gambling-started-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  gamblingCompletedIcon = L.icon({
    iconUrl: `${this.markersPath}/gambling-completed-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  locationIcon = L.icon({
    iconUrl: `${this.markersPath}/location-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  locationStartedIcon = L.icon({
    iconUrl: `${this.markersPath}/location-started-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  locationCompletedIcon = L.icon({
    iconUrl: `${this.markersPath}/location-completed-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  miscIcon = L.icon({
    iconUrl: `${this.markersPath}/misc-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  miscStartedIcon = L.icon({
    iconUrl: `${this.markersPath}/misc-started-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  miscCompletedIcon = L.icon({
    iconUrl: `${this.markersPath}/misc-completed-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  socialIcon = L.icon({
    iconUrl: `${this.markersPath}/social-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  socialStartedIcon = L.icon({
    iconUrl: `${this.markersPath}/social-started-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  socialCompletedIcon = L.icon({
    iconUrl: `${this.markersPath}/social-completed-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });

  objectIcon = L.icon({
    iconUrl: `${this.markersPath}/object-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  parkingIcon = L.icon({
    iconUrl: `${this.markersPath}/parking-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  restaurantIcon = L.icon({
    iconUrl: `${this.markersPath}/restaurant-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  walkingIcon = L.icon({
    iconUrl: `${this.markersPath}/walking-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  wcIcon = L.icon({
    iconUrl: `${this.markersPath}/wc-marker.png`,
    iconSize: this.customIconSize, 
    iconAnchor: this.customIconAnchor, 
  });
  //#endregion

  constructor(private mapService: MapService) {
    this.enableClicks = true;
    this.markType = 'Key point';
    this.toggleOff = false;
    this.allowMultipleMarkers = true;
    this.drawRadiusOnClick = false;
    this.fitSelectedRoutes = true;
    this.rememberClick = false;
  }

  public handleButtonClick(): void {
    this.markType = this.markType === 'Key point' ? 'Object' : 'Key point';
    alert(this.markType);
  }

  private initMap(): void {
    this.map = L.map('map', {
      center: [45.2396, 19.8227],
      zoom: 13,
    });

    const tiles = L.tileLayer(
      'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
      {
        maxZoom: 18,
        minZoom: 3,
        attribution:
          '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      }
    );
    tiles.addTo(this.map);
    this.markerLayer = new L.LayerGroup();
    this.drawLayer = new L.LayerGroup();
    this.markerLayer.addTo(this.map);
    this.drawLayer.addTo(this.map);

    if (this.enableClicks) {
      this.registerOnClick();
    }
    if (this.routeQuery) {
      this.setRoute();
    }
    if (this.markerPosition) {
      this.setMarker(this.markerPosition.latitude, this.markerPosition.longitude, this.markerPosition.color);
    }
    if(this.markerPositions && this.markerPositions.length > 0) {
      this.markerPositions.forEach((marker) => {
        this.setMarker(marker.latitude, marker.longitude, marker.color, marker.title);
      });
    }
  }

  ngAfterViewInit(): void {
    let DefaultIcon = L.icon({
      iconUrl: 'https://unpkg.com/leaflet@1.6.0/dist/images/marker-icon.png',
    });

    L.Marker.prototype.options.icon = DefaultIcon;
    this.initMap();
  }

  ngOnChanges(): void {
    if (this.map) {
      this.setRoute();
      this.clearDrawings();
      this.clearMarkers();
      if (this.markerPosition) {
        this.setMarker(this.markerPosition.latitude, this.markerPosition.longitude, this.markerPosition.color, '');
        this.map.panTo(L.latLng(this.markerPosition.latitude, this.markerPosition.longitude));
      }

      if(this.markerPositions && this.markerPositions.length > 0) {
        this.markerPositions.forEach((marker) => {
          this.setMarker(marker.latitude, marker.longitude, marker.color, marker.title);
          if(marker.radiusSize && marker.radiusSize > 0) 
            this.setRadius(marker.latitude, marker.longitude, marker.radiusSize, marker.color);
        });
      }

      if(this.rememberClick) {
        this.drawPreviousClickMarker();
      }

      if(this.drawRadiusOnClick && this.radiusSize && this.radiusSize > 0) {
        this.clearDrawings();
        this.setRadius(this.clickMarker.getLatLng().lat, this.clickMarker.getLatLng().lng, this.radiusSize, 'red');
      }
    }
  }

  search(): void {
    this.mapService.search('Strazilovska 19, Novi Sad').subscribe({
      next: (result) => {
        console.log(result);
        L.marker([result[0].lat, result[0].lon])
          .addTo(this.map)
          .bindPopup('Pozdrav iz Strazilovske 19.')
          .openPopup();
      },
      error: () => { },
    });
  }

  registerOnClick(): void {
    this.map.on('click', (e: any) => {
      const coord = e.latlng;
      const lat = coord.lat;
      const lng = coord.lng;
      this.mapService.reverseSearch(lat, lng).subscribe((res) => {
        console.log(res.display_name);
      });
      console.log(
        'You clicked the map at latitude: ' + lat + ' and longitude: ' + lng
      );

      if (!this.allowMultipleMarkers) {
        this.clearMarkers();
        this.clearDrawings();
      }

      if (this.markType == 'Object') {
        const customIcon = L.icon({
          iconUrl: 'https://www.pngall.com/wp-content/uploads/2017/05/Map-Marker-Free-Download-PNG.png',
          iconSize: this.customIconSize,
          iconAnchor: [16, 16],
        });
        this.clickMarker = L.marker([lat, lng], { icon: customIcon }).addTo(this.markerLayer);
        alert(this.clickMarker.getLatLng());
      } else {
        //this.clickMarker = new L.Marker([lat, lng]).addTo(this.markerLayer);
        this.setMarker(lat, lng, 'walking');
        this.clickEvent.emit([lat, lng]);
      }

      if(this.drawRadiusOnClick && this.radiusSize && this.radiusSize > 0) {
        this.setRadius(lat, lng, this.radiusSize, 'red');
      }
    });
  }

  drawPreviousClickMarker(){
    this.setMarker(this.clickMarker.getLatLng().lat, this.clickMarker.getLatLng().lng);
  }

  clearMarkers(): void {
    this.markerLayer.eachLayer((layer) => {
      layer.remove();
    })
  }

  clearDrawings(): void {
    this.drawLayer.eachLayer((layer) => {
      layer.remove();
    })
  }

  setRoute(): void {
    if (this.routeQuery && this.routeQuery.keypoints.length > 1) {
      var routesFoundEvent = this.routesFoundEvent;

      if (this.routeControl) {
        this.routeControl.remove(); //Removes previous legend 
      }

      let lwaypoints = [];
      for (let i = 0; i < this.routeQuery.keypoints.length; i++) {
        let k = this.routeQuery.keypoints[i];
        let latLng = L.latLng(k.latitude, k.longitude);
        lwaypoints.push(latLng);

        // Create a marker for the keypoint and add it to the map
        let marker = L.marker([k.latitude, k.longitude]);
        let tooltipText = k.name;
        if (i === 0) {
          tooltipText = 'Start: ' + tooltipText;
        } else if (i === this.routeQuery.keypoints.length - 1) {
          tooltipText = 'Finish: ' + tooltipText;
        }
        marker.bindTooltip(tooltipText, { permanent: true }).openTooltip();
        marker.addTo(this.map);
      }

      let profile = '';
      switch (this.routeQuery.transportType) {
        case TransportType.WALK:
          profile = 'mapbox/walking';
          break;

        case TransportType.CAR:
          profile = 'mapbox/driving';
          break;

        default:
          profile = 'mapbox/cycling';
          break;
      }

      this.routeControl = L.Routing.control({
        waypoints: lwaypoints,
        router: L.routing.mapbox(environment.mapBoxApiKey, { profile: profile }),
        fitSelectedRoutes: this.fitSelectedRoutes
      }).addTo(this.map);

      this.routeControl.on('routesfound', function (e: any) {
        var routes = e.routes;
        var summary = routes[0].summary;
        let routeInfo: RouteInfo = {
          distance: summary.totalDistance / 1000,
          duration: Math.ceil(summary.totalTime / 60)
        }
        routesFoundEvent.emit(routeInfo);
      });
    }
  }

  setMarker(lat: number, lng: number, color: string = 'blue', title: string = '', isTitlePermanent: boolean = false) {
    let markerIcon = this.blueIcon;
    markerIcon = this.getMarkerIcon(color);
    
    let newMarker = new L.Marker([lat, lng], {icon: markerIcon});
    if(title && title !== '') {
      newMarker.bindTooltip(title, { permanent: isTitlePermanent }).openTooltip();
    }
    newMarker.addTo(this.markerLayer);
  }

  setRadius(centerLat: number, centerLng: number, radius: number, color: string = 'red'): void {
    L.circle([centerLat, centerLng], {
      color: color,
      fillColor: color,
      fillOpacity: 0.2,
      radius: radius * 1000
    }).addTo(this.drawLayer);
  }

  getMarkerIcon(color: string):L.Icon {
    // would be clean if this could work:
    // const iconName = `${color}Icon`
    // return this[iconName]; // this['redIcon'] works -.-

    switch(color)
    {
      case 'red': return this.redIcon;
      case 'yellow': return this.yellowIcon;
      case 'green': return this.greenIcon;
      case 'blue': return this.blueIcon;
      case 'gambling': return this.gamblingIcon;
      case 'gambling-started': return this.gamblingStartedIcon;
      case 'gambling-completed': return this.gamblingCompletedIcon;
      case 'location': return this.locationIcon;
      case 'location-started': return this.locationStartedIcon;
      case 'location-completed': return this.locationCompletedIcon;
      case 'misc': return this.miscIcon;
      case 'misc-started': return this.miscStartedIcon;
      case 'misc-completed': return this.miscCompletedIcon;
      case 'social': return this.socialIcon;
      case 'social-started': return this.socialStartedIcon;
      case 'social-completed': return this.socialCompletedIcon;
      case 'object': return this.objectIcon;
      case 'parking': return this.parkingIcon;
      case 'restaurant': return this.restaurantIcon;
      case 'walking': return this.walkingIcon;
      case 'wc': return this.wcIcon;
      default: return this.blueIcon;
    }
  }
}
