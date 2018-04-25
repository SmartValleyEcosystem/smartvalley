export class FileUploaderHelper {
  private static photoExtensions = ['jpeg', 'jpg', 'png'];
  private static cvExtensions = ['pdf', 'doc', 'docx'];

  public static checkImageExtensions(imageFile: File): boolean {
    const expansion = imageFile.name.split('.').last().toLowerCase();
    return imageFile.type.split('/').first() === 'image' && this.photoExtensions.some(i => i === expansion);
  }

  public static checkCVExtensions(imageFile: File): boolean {
    const expansion = imageFile.name.split('.').last().toLowerCase();
    return this.cvExtensions.some(i => i === expansion);
  }
}
