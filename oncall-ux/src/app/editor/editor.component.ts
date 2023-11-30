import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss']
})
export class EditorComponent implements OnInit {
  team: any;
  year: any;
  week: any;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.team = this.route.snapshot.paramMap.get('team');
    this.year = this.route.snapshot.paramMap.get('year');
    this.week = this.route.snapshot.paramMap.get('week');
  }

}
