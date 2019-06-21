import { NavigationStart } from '@angular/router';
import { Component, OnInit, ViewChild, AfterContentChecked, Renderer } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from './../_models';
import { UserService, AuthenticationService } from './../_services';
import { isNgTemplate } from '@angular/compiler';
declare var $;
@Component({ templateUrl: 'home.component.html' })
export class HomeComponent implements OnInit {
  @ViewChild('dataTable', { static: true}) table;
  @ViewChild('searchText', {static: true}) searchInput;
  dataTable: any;
  dtOptions: any;
  input: any;
  currentUser: User;
  users = [];
  allUsers = [];

  hasAttachedListener = false;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private renderer: Renderer
    ) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    ngOnInit() {
        this.loadAllUsers();
      }

    deleteUser(id: number) {
        this.userService.delete(id)
            .pipe(first())
            .subscribe(() => this.loadAllUsers());
    }

    private loadAllUsers() {
        this.userService.getAll()
            .pipe(first())
            .subscribe(users =>
              this.bindDataTable(users)
              );
    }

    searchKeyword() {
      this.input = $(this.searchInput.nativeElement);
      const search = this.input.val();
      if (search.trim().length === 0) {
        this.users = this.allUsers;
      } else {
        this.users = Object.assign([], this.allUsers).filter(item =>
          this.returnUsername(item, search));

        // this.users = Object.assign([], this.users).filter(item =>
        //     this.returnFirstname(item, search));

        // this.users = Object.assign([], this.users).filter(item =>
        //       this.returnLastname(item, search));
      }
    }

    returnUsername(item, search) {
      return item.username.toLowerCase().indexOf(search.toLowerCase()) > -1
      || item.firstName.toLowerCase().indexOf(search.toLowerCase()) > -1
      || item.lastName.toLowerCase().indexOf(search.toLowerCase()) > -1;
    }

    // returnFirstname(item, search) {
    //   return item.firstname.toLowerCase().indexOf(search.toLowerCase()) > -1;
    // }

    // returnLastname(item, search) {
    //   return item.lastname.toLowerCase().indexOf(search.toLowerCase()) > -1;
    // }

    bindDataTable(users) {
      this.allUsers = users;
      this.users = users;
    }

    //   this.dtOptions = {
    //     //data: users,
    //     "paging": true,
    //     "ordering": true,
    //     "info": true,
    //     "pagingType": "full_numbers",
    //     // columns: [
    //     //   { "data": "username" },
    //     //   { "data": "firstName" },
    //     //   { "data": "lastName" },
    //     //   { "data": "id" }
    //     // ],
    //     // "columnDefs": [{
    //     //   "targets": [0],
    //     //   "visible": true,
    //     //   "searchable": true
    //     // },
    //     // {
    //     //     "targets": [3],
    //     //     "orderable": false,
    //     //     "render": function (data, type, full, meta) {
    //     //         return '<a #deleteButton (click)=deleteUser(' + data + ') class="text-danger">Delete</a>';
    //     //         //return `<a #deleteButton class="text-danger">Delete</a>`;
    //     //     }
    //     // }]
    //   };

    //   this.dataTable = $(this.table.nativeElement);
    //   this.dataTable.DataTable(this.dtOptions);
    //   // console.log('this.dataTable:', this.dataTable);
    //   // console.log('this.dataTable[0]:', this.dataTable[0]);

    //   // console.log('this.dtOptions:',this.dtOptions);
    //   // this.dtOptions.columnDefs.forEach(element => {
    //   //   console.log('element:',element);
    //   // });
    // }

  //   ngAfterContentChecked() {
  //     console.log('AfterContentChecked');
  //     console.log(this.deleteButton); // ? 'is myButton' : "myButton undefined");
  //     if(this.deleteButton && !this.hasAttachedListener){
  //       let simple = this.renderer.listen(this.deleteButton.nativeElement, 'click', (evt) => {
  //           console.log('Clicking the button', evt);
  //       });
  //       this.hasAttachedListener = true;
  //     }
  //   }

  //   ngAfterViewChecked() {
  //     console.log('ngAfterViewChecked');
  //     console.log(this.deleteButton);
  //     if(this.deleteButton && !this.hasAttachedListener){
  //         let simple = this.renderer.listen(this.deleteButton.nativeElement, 'click', (evt) => {
  //             console.log('Clicking the button', evt);
  //         });
  //         this.hasAttachedListener = true;
  //     }
  // }

    // ngAfterViewInit() {
    //   let simple = this.renderer.listen(this.myButton.nativeElement, 'click', (evt) => {
    //     console.log('Clicking the button', evt);
    //   });
    // }

  }
