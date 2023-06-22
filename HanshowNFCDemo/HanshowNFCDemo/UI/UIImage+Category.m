//
//  UIButton+UIButton_Ext.m
//  
//
//  Created by hanshow on 2023/5/16.
//

#import "UIImage+Category.h"

@implementation UIImage (UIImage_Ext)
- (UIImage *)resizedImage:(CGSize)newSize {
    UIGraphicsBeginImageContextWithOptions(newSize, NO, 0);
    [self drawInRect:CGRectMake(0, 0, newSize.width, newSize.height)];
    UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    return newImage;
}
@end
