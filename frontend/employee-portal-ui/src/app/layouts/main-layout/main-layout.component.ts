import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SidePanelComponent } from "../side-panel/side-panel.component";

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    RouterModule, // Import RouterModule
    SidePanelComponent
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent {}