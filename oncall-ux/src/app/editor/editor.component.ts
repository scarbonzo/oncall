import { ActivatedRoute } from '@angular/router'
import { Component, OnInit } from '@angular/core';
import { OncallService } from '../oncall.service';
import { Oncall } from '../oncall';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss']
})
export class EditorComponent implements OnInit {
  team: any;
  year: any;
  week: any;
  core: any;
  web: any;

  coreTeam = [ "James", "Rich", "Dan", "Eric", "Tim", "Raj" ];
  webTeam = [ "Sri", "Corrado", "Pedro", "Mikhail", "Mark" ];

  constructor(private oncallService: OncallService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.team = this.route.snapshot.paramMap.get('team');
    this.year = this.route.snapshot.paramMap.get('year');
    this.week = this.route.snapshot.paramMap.get('week');
    this.UpdateWeek();
  }

  UpdateWeek() {
    this.oncallService
      .getOncall('Core', this.year, this.week)
      .subscribe(data => {  this.core = data;
    });
    this.oncallService
      .getOncall('Web', this.year, this.week)
      .subscribe(data => {  this.web = data;
    });
  }
}
