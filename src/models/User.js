export class User {
  constructor(email, fullname, phone, hashpass, address) {
    this.email = email
    this.fullname = fullname
    this.hashpass = hashpass
    this.phone = phone
    this.address = address
    this.role = 1
  }
}
