export interface Initializable {
  initializeAsync(): Promise<void>;
}
