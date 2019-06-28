CREATE SCHEMA GeektextDB2017;

create table GeektextDB2017.Users
(Username VARCHAR(25) not null,
 UserPassword varchar(25) not null,
 UserFirst varchar(50) not null,
 UserMiddle varchar(50) null,
 UserLast varchar(50) not null,
 Nickname varchar(50) null,
 Email varchar(50) not null,
 primary key (Username));

 create table GeektextDB2017.Book
 (ISBN varchar(13) not null,
  Title varchar(150) not null,
  Price decimal(7,2) not null,
  BookDescription varchar(max) null,
  PublisherName varchar(50) null,
  DatePublished date null,
  primary key (ISBN));

  create table GeektextDB2017.Author
  (AuthorID smallint not null,
   AuthorFirst varchar(50) not null,
   AuthorMiddle varchar(50) null,
   AuthorLast varchar(50) not null,
   Bio varchar(max) null,
   primary key (AuthorID));

   create table GeektextDB2017.Wrote
   (ISBN varchar(13) not null,
    AuthorID smallint not null,
	AuthorSequence decimal(1,0) null,
	primary key (ISBN, AuthorID),
	foreign key (ISBN) references GeektextDB2017.Book(ISBN),
	foreign key (AuthorID) references GeektextDB2017.Author(AuthorID));

	create table GeektextDB2017.CreditCard
	(CCNumber varchar(19) not null,
	 Username varchar(25) not null,
	 FirstName varchar(50) not null,
	 LastName varchar(50) not null,
	 CVV varchar(4) not null,
	 ExpDate date not null,
	 primary key (CCNumber, Username),
	 foreign key (Username) references GeektextDB2017.Users(Username));


CREATE TABLE GeektextDB2017.Wishlist
( Username VARCHAR(25) not null,
  ISBN varchar(13) not null,
  Quantity decimal(2, 0) not null,
  WishlistName varchar(25) not null,
  Preferred bit not null,
    primary key (Username, ISBN, WishlistName),
    foreign key (Username) references GeektextDB2017.Users(Username),
	foreign key (ISBN) references GeektextDB2017.Book(ISBN));

create table GeektextDB2017.ShoppingCart
(Username varchar(25) not null,
 ISBN varchar(13) not null,
 PriceEach decimal(7, 2) not null,
 Quantity decimal (2, 0) not null,
 primary key (Username, ISBN),
 foreign key (Username) references GeektextDB2017.Users(Username),
 foreign key (ISBN) references GeektextDB2017.Book(ISBN));

CREATE TABLE GeektextDB2017.ShippingAddress 
(Username VARCHAR(25) not null,
 AddressNum Decimal(2, 0) not null,
 PreferredAddress bit not null,
  Street varchar(50) not null,
  Apt decimal(4, 0),
  City varchar(25) not null,
  ZipCode varchar(15) not null,
  Country varchar(25) not null,
  constraint ShippingAddress_pk
    primary key (Username, AddressNum),
  constraint ShippingAddressUser_fk
    foreign key (Username) references GeektextDB2017.Users(Username));

CREATE TABLE GeektextDB2017.HomeAddress 
(Username VARCHAR(25) not null,
  Street varchar(50) not null,
  Apt decimal(4, 0),
  City varchar(25) not null,
  ZipCode varchar(15) not null,
  Country varchar(25) not null,
    primary key (Username),
    foreign key (Username) references GeektextDB2017.Users(Username));

create table GeektextDB2017.Purchased
(Username varchar(25) not null,
 ISBN varchar(13) not null,
 primary key (Username, ISBN),
 foreign key (ISBN) references GeektextDB2017.Book(ISBN),
 foreign key (Username) references GeektextDB2017.Users(Username));

 create table GeektextDB2017.Reviews
 (ISBN varchar(13) not null,
  Username varchar(25) not null,
  Rating decimal(1,0) not null,
  Comment varchar(max) null,
  DatePosted date not null
  primary key (ISBN, Username),
  foreign key (ISBN) references GeektextDB2017.Book(ISBN),
  foreign key (Username) references GeektextDB2017.Users(Username));

  create table GeektextDB2017.Genre
  (GenreName varchar(25) not null,
   primary key (GenreName));

   create table GeektextDB2017.BookGenre
   (GenreName varchar(25) not null,
    ISBN varchar(13) not null,
	primary key (GenreName, ISBN),
	foreign key (GenreName) references GeektextDB2017.Genre(GenreName),
	foreign key (ISBN) references GeektextDB2017.Book(ISBN));
