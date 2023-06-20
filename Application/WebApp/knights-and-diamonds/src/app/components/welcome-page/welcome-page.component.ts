import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',
  styleUrls: ['./welcome-page.component.css'],
})
export class WelcomePageComponent implements OnInit {
  opacity = 0;

  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Calculate the scroll position
    const scrollPosition =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;

    // Calculate the opacity based on the scroll position
    const maxOpacity = 1; // Maximum opacity when scrolled to the bottom
    const opacity =
      (scrollPosition /
        (document.documentElement.scrollHeight - window.innerHeight)) *
      maxOpacity;

    // Update the opacity value
    this.opacity = opacity * 2;
  }
  constructor() {}

  ngOnInit(): void {}
}
