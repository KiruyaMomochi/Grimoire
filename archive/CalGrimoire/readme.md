Database:
- User
- Reserve
- Current

```
Table users as U {
  id string [pk]
  displayName varchar
}

Table groups as G {
  id string [pk]
  clanId int [ref: - C.id]
}

Table clans as C {
  id int [pk]
  displayName varchar
}

Table admins as A {
  userId string [ref: > U.id]
  clanId int [ref: > C.id]
}

Table current {
  clanId int [ref: - C.id]
  lap int
  order int
}

Table records as R {
  id int [pk]
  clanId int [ref: > C.id]
  userId string [ref: > U.id]
  lap int
  order int
  comment int
  makeup bool
  failed bool
}

Table reserves {
  id int [pk]
  clanId int [ref: > C.id]
  userId string [ref: > U.id]
  lap int
  order int
  comment int
  makeup bool
}

View 
```
