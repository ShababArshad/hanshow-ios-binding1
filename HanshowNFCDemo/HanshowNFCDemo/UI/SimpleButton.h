//
//  SimpleButton.h
//  EVClub
//
//  Created by Eric on 15/11/18.
//  Copyright © 2015年 BitRice. All rights reserved.
//

#import <UIKit/UIKit.h>
typedef NS_ENUM(UInt8,ImageLocation){
    kImageLocationLeft,/**<图片在文字左侧*/
    kImageLocationRight,/**<图片在文字右侧*/
    kImageLocationTop,/**<图片在文字上方*/
    kImageLocationBottom,/**<图片在文字下方*/
    
};
typedef NS_ENUM(UInt8,AlignmentCenterMode){
    kAlignmentCenterModeUnion,/**<以图片文字合并中心作为中点*/
    kAlignmentCenterModeTextCenter/**<以文字中心作为中点*/
};
@interface SimpleButton : UIButton
@property(nonatomic) ImageLocation imageLocation;
@property(nonatomic) AlignmentCenterMode alignmentCenterMode;
@property(nonatomic,strong) UIImage *customImage;
@property(nonatomic,strong) UIImageView *customImageView;/**<如果指定了customImage,button.imageView就是customImageView*/
@property(nonatomic) CGFloat titleImageGap;/**<图片与文字的间隔*/
@property(nonatomic) CGFloat limitWidth;
@property(nonatomic) CGFloat limitTextWidth;
-(void)setImageLocation:(ImageLocation)imageLocation
    AlignmentCenterMode:(AlignmentCenterMode)alignmentCenterMode
                    Gap:(CGFloat)gap;
-(void)setImageLocation:(ImageLocation)imageLocation Gap:(CGFloat)gap;
@end
