import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { MainComponent } from '../pages/main/main.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { LayoutRouting } from './layout.routing';
import { LayoutComponent } from './layout/layout.component';


@NgModule({
    declarations: [
        HeaderComponent,
        FooterComponent,
        LayoutComponent,
        MainComponent
    ],
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        LayoutRouting
    ],
    exports: [
        HeaderComponent,
        FooterComponent,
        LayoutComponent
    ]
})
export class LayoutModule { }
