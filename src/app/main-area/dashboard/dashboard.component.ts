import { AfterViewInit, ChangeDetectorRef, Component, Inject, NgZone, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalysisService } from '../../../generated/services/analysis/analysis.service';
import { DatePipe, DecimalPipe, isPlatformBrowser } from '@angular/common';

import * as am4core from '@amcharts/amcharts4/core';
import * as am4maps from '@amcharts/amcharts4/maps';
import am4themes_animated from '@amcharts/amcharts4/themes/animated';
import am4geodata_worldLow from '@amcharts/amcharts4-geodata/worldLow';
import { ApplicationsByLocationResponseModel } from '../../../models/analysis/analysis.model';
import { RouterLink } from '@angular/router';
import { RoleService } from '../../../generated/services/role/role.service';
import { RoleDetailsModel } from '../../../models/roles/role.model';



@Component({
  selector: 'app-dashboard',
  imports: [
    DecimalPipe,
    DatePipe,
    RouterLink
],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy, AfterViewInit {

  protected averageTimeToFill: number = 0;
  protected totalRoles: number = 0;
  protected openRoles: number = 0;
  protected closedRoles: number = 0;
  protected applicationByLocation: ApplicationsByLocationResponseModel[] = [];
  protected recentRoles: RoleDetailsModel[] = [];
  private unsubscribe: Subscription[] = [];

  constructor(private analysisService: AnalysisService, private cdr: ChangeDetectorRef, private roleService: RoleService,
    @Inject(PLATFORM_ID) private platformId: Object, private zone: NgZone
  ) { }

  private getJobDistribution() {
    const sub = this.analysisService.getJobStatusDistribution().subscribe({
      next: response => {
        this.totalRoles = response.reduce((sum, item) => sum + item.count, 0);
        this.openRoles = response.find(item => item.status === 1)?.count ?? 0;
        this.closedRoles = response.find(item => item.status === 2)?.count ?? 0;
        this.cdr.detectChanges();
      },
      error: () => {
        this.totalRoles = this.openRoles = this.closedRoles = 0;
        this.cdr.detectChanges();
      }
    });

    this.unsubscribe.push(sub);
  }

  private getAverageTimeToFIll() {
    const sub = this.analysisService.getAverageTimeToFill().subscribe({
      next: response => {
        this.averageTimeToFill = Math.round(response);
        this.cdr.detectChanges();
      },
      error: () => {
        this.averageTimeToFill = 0;
        this.cdr.detectChanges();
      }
    });

    this.unsubscribe.push(sub);
  }

  private getApplicationsByLocation() {
    const sub = this.analysisService.getApplicationsByLocation().subscribe({
      next: response => {
        this.applicationByLocation = response;
        this.cdr.detectChanges();
        this.drawWorldMap();
      },
      error: () => {
        this.applicationByLocation = [];
        this.cdr.detectChanges();
        this.drawWorldMap();
      }
    });

    this.unsubscribe.push(sub);
  }

  private getRecentApplications() {
    const sub = this.roleService.recentRoles().subscribe({
      next: response => {
        this.recentRoles = response.data.items;
        this.cdr.detectChanges();
      },
      error: () => {
        this.recentRoles = [];
        this.cdr.detectChanges();
      },
    });

    this.unsubscribe.push(sub);
  }

  ngOnInit(): void {
    this.getJobDistribution();
    this.getAverageTimeToFIll();
    this.getApplicationsByLocation();
    this.getRecentApplications();
  }

  ngOnDestroy(): void {
    this.unsubscribe.forEach(s => s.unsubscribe());
  }

  // Run the function only in the browser
  browserOnly(f: () => void) {
    if (isPlatformBrowser(this.platformId)) {
      this.zone.runOutsideAngular(() => {
        f();
      });
    }
  }

  drawWorldMap() {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create map instance
    var chart = am4core.create("world-map", am4maps.MapChart);
    // Set map definition
    chart.geodata = am4geodata_worldLow;

    // Set projection
    chart.projection = new am4maps.projections.Miller();

    // Create map polygon series
    var polygonSeries = chart.series.push(new am4maps.MapPolygonSeries());

    // Exclude Antartica
    polygonSeries.exclude = ["AQ"];

    // Make map load polygon (like country names) data from GeoJSON
    polygonSeries.useGeodata = true;

    // Configure series
    var polygonTemplate = polygonSeries.mapPolygons.template;
    polygonTemplate.fill = am4core.color('#827af3dd');

    polygonTemplate.tooltipText = "{name}";
    polygonTemplate.polygon.fillOpacity = 0.6;


    // Create hover state and set alternative fill color
    var hs = polygonTemplate.states.create("hover");
    hs.properties.fill = chart.colors.getIndex(0);

    // Add image series
    var imageSeries = chart.series.push(new am4maps.MapImageSeries());
    imageSeries.mapImages.template.propertyFields.longitude = "longitude";
    imageSeries.mapImages.template.propertyFields.latitude = "latitude";
    imageSeries.mapImages.template.tooltipText = "{title}";
    imageSeries.mapImages.template.propertyFields.url = "url";

    var circle = imageSeries.mapImages.template.createChild(am4core.Circle);
    circle.radius = 5;
    circle.propertyFields.fill = "color";

    var circle2 = imageSeries.mapImages.template.createChild(am4core.Circle);
    circle2.radius = 5;
    circle2.propertyFields.fill = "color";


    circle2.events.on("inited", function (event) {
      animateBullet(event.target);
    })


    function animateBullet(circle: any) {
      var animation = circle.animate([{ property: "scale", from: 3, to: 10 }, { property: "opacity", from: 1, to: 0 }], 1000, am4core.ease.circleOut);
      animation.events.on("animationended", function (event: any) {
        animateBullet(event.target.object);
      })
    }

    var colorSet = new am4core.ColorSet();

    imageSeries.data = [
      {
        "title": `Dallas: ${this.applicationByLocation.find(x => x.location.toLowerCase() === 'dallas')?.applications ?? 0} applications`,
        "latitude": 32.7767,
        "longitude": -96.7970,
        "color": colorSet.next()
      },
      {
        "title": `Gaborene: ${this.applicationByLocation.find(x => x.location.toLowerCase() === 'gaborene')?.applications ?? 0} applications`,
        "latitude": -24.6580,
        "longitude": 25.9077,
        "color": colorSet.next()
      },
      {
        "title": `Manila: ${this.applicationByLocation.find(x => x.location.toLowerCase() === 'manila')?.applications ?? 0} applications`,
        "latitude": 14.5995,
        "longitude": 120.9842,
        "color": colorSet.next()
      },
      {
        "title": `Lagos: ${this.applicationByLocation.find(x => x.location.toLowerCase() === 'lagos')?.applications ?? 0} applications`,
        "latitude": 6.5244,
        "longitude": 3.3792,
        "color": colorSet.next()
      },
      {
        "title": `Pune: ${this.applicationByLocation.find(x => x.location.toLowerCase() === 'pune')?.applications ?? 0} applications`,
        "latitude": 18.5246,
        "longitude": 73.8786,
        "color": colorSet.next()
      },
    ];

  }

  ngAfterViewInit() {
    // Chart code goes in here
    this.browserOnly(() => {
      this.drawWorldMap();
    });
  }
}
