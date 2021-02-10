export class Message {
  message: string;
  from: string;
  to: string;
  time: string;
  constructor(message: string, from: string, to: string, stamp: string) {
    this.message = message;
    this.from = from;
    this.to = to;
    this.time = stamp;
  }
}
