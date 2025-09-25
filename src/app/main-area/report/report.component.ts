import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';

import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from '@amcharts/amcharts4/charts';
import am4themes_animated from '@amcharts/amcharts4/themes/animated';
import { Subscription } from 'rxjs';
import { AnalysisService } from '../../../generated/services/analysis/analysis.service';
import { OpenRolesDurationResponseModel } from '../../../models/analysis/analysis.model';

@Component({
  selector: 'app-report',
  imports: [],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss'
})
export class ReportComponent implements OnInit, OnDestroy {

  private unsubscribe: Subscription[] = [];

  constructor(private analysisService: AnalysisService, private cdr: ChangeDetectorRef) {}

  private getOpenRoleDurationInformation() {
    const sub = this.analysisService.getOpenRolesDuration().subscribe({
      next: response => {
        this.drawOpenRoleDuration(response);
      }
    });

    this.unsubscribe.push(sub);
  }

  private drawOpenRoleDuration(rolesInfo: OpenRolesDurationResponseModel[]) {
    am4core.useTheme(am4themes_animated);
    rolesInfo = Array.from(new Map(rolesInfo.map(item => [item.roleTitle, item])).values());
    rolesInfo = rolesInfo.sort((a,b) => b.openDays - a.openDays);
		var e = am4core.create("am-simple-chart", am4charts.XYChart);
		e.colors.list = [am4core.color("#827af3")], e.data = rolesInfo.slice(0, 10).map(x => { return { roles: x.roleTitle, days: x.openDays } });
		var t = e.xAxes.push(new am4charts.CategoryAxis);
		t.dataFields.category = "roles", t.renderer.grid.template.location = 0, t.renderer.minGridDistance = 30, t.renderer.labels.template.adapter.add("dy", function(e, t) {
			//@ts-ignore
      return t.dataItem && !0 & t.dataItem.index ? e + 25 : e
		});
		e.yAxes.push(new am4charts.ValueAxis);
		var a = e.series.push(new am4charts.ColumnSeries);
		a.dataFields.valueY = "days", a.dataFields.categoryX = "roles", a.name = "Days", a.columns.template.tooltipText = "{categoryX}: [bold]{valueY} days[/]", a.columns.template.fillOpacity = .8;
		var n = a.columns.template;
		n.strokeWidth = 2, n.strokeOpacity = 1
  }

  ngOnInit(): void {
    this.getOpenRoleDurationInformation();
    
  }

  ngOnDestroy(): void {
    this.unsubscribe.forEach(s => s.unsubscribe());
  }
}
