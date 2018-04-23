export class ImageUploaderHelper {
  private static photoExtensions = ['jpeg', 'jpg', 'png'];

  public static checkImageExtensions(imageFile: File): boolean {
    const expansion = imageFile.name.split('.').last().toLowerCase();
    return imageFile.type.split('/').first() === 'image' && this.photoExtensions.some(i => i === expansion);
  }
}
