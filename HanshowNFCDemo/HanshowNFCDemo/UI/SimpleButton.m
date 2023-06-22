//
//  SimpleButton.m
//  EVClub
//
//  Created by Eric on 15/11/18.
//  Copyright © 2015年 BitRice. All rights reserved.
//

#import "SimpleButton.h"
@interface SimpleButton()
@end
@implementation SimpleButton
-(void)layoutSubviews
{
    [super layoutSubviews];
    [self update];
}
-(void)setImageLocation:(ImageLocation)imageLocation Gap:(CGFloat)gap{
    [self setImageLocation:imageLocation AlignmentCenterMode:kAlignmentCenterModeUnion Gap:gap];
}
-(void)setImageLocation:(ImageLocation)imageLocation
    AlignmentCenterMode:(AlignmentCenterMode)alignmentCenterMode
                    Gap:(CGFloat)gap
{
    self.imageLocation = imageLocation;
    self.alignmentCenterMode = alignmentCenterMode;
    self.titleLabel.textAlignment = NSTextAlignmentCenter;
    self.titleImageGap = gap;
}
-(UIImageView*)imageView
{
    if(_customImage)
    {
        if(!_customImageView)
        {
            self.customImageView = [[UIImageView alloc]initWithImage:_customImage];
            [self addSubview:_customImageView];
        }
        return _customImageView;
    }
    else return [super imageView];
}
-(void)update
{
    CGFloat width = self.frame.size.width;
    CGFloat height = self.frame.size.height;
    [self.titleLabel sizeToFit];
    CGFloat theGap = _titleImageGap;
    if(self.titleLabel.text.length <= 0 || self.imageView.image == nil) theGap = 0;
    if(_alignmentCenterMode == kAlignmentCenterModeTextCenter)
    {
        [self.titleLabel setCenter:CGPointMake(width/2, height/2)];
    }
    else if(_alignmentCenterMode == kAlignmentCenterModeUnion)
    {
        CGSize minSize = [self minSize];
        CGFloat x = width/2;
        CGFloat y = height/2;
        switch (_imageLocation) {
            case kImageLocationLeft:{x = width/2 - minSize.width/2 + (minSize.width - self.titleLabel.frame.size.width/2);}break;
            case kImageLocationRight:{x = width/2 - minSize.width/2 + self.titleLabel.frame.size.width/2;}break;
            case kImageLocationTop:{y = height/2 - minSize.height/2 + (minSize.height - self.titleLabel.frame.size.height/2);}break;
            case kImageLocationBottom:{y = height/2 - minSize.height/2 + self.titleLabel.frame.size.height/2;}break;
            default:break;
        }
        [self.titleLabel setCenter:CGPointMake(x, y)];
    }
    switch (_imageLocation) {
        case kImageLocationLeft:{self.imageView.center = CGPointMake(self.titleLabel.frame.origin.x  - theGap - self.imageView.frame.size.width/2, self.titleLabel.center.y);}break;
        case kImageLocationRight:{self.imageView.center = CGPointMake(self.titleLabel.frame.origin.x + self.titleLabel.frame.size.width + theGap + self.imageView.frame.size.width/2, self.titleLabel.center.y);}break;
        case kImageLocationTop:{self.imageView.center = CGPointMake(self.titleLabel.center.x, self.titleLabel.frame.origin.y - theGap - self.imageView.frame.size.height/2);}break;
        case kImageLocationBottom:{self.imageView.center = CGPointMake(self.titleLabel.center.x, self.titleLabel.frame.origin.y + self.titleLabel.frame.size.height + theGap + self.imageView.frame.size.height/2);}break;
        default:break;
    }
    
}
-(void)sizeToFit
{
    CGSize minSize = [self minSize];
    self.frame = (CGRect){self.frame.origin,minSize};
    [self update];
}

-(CGSize)minSize
{
    [self.titleLabel sizeToFit];
    if(!_customImage) [self.imageView sizeToFit];
    CGFloat theGap = _titleImageGap;
    if(self.titleLabel.text.length <= 0 || self.imageView.image == nil) theGap = 0;
    
    if(_limitWidth > 0)
    {
        
        
        if(_imageLocation == kImageLocationTop ||
           _imageLocation == kImageLocationBottom){
            CGRect frame = self.frame;
            frame.size.width = MIN(_limitWidth, MAX(CGRectGetWidth(self.titleLabel.frame), CGRectGetWidth(self.imageView.frame)));
            self.frame = frame;
        }
        else if(_imageLocation == kImageLocationLeft){
            if(_limitWidth < (CGRectGetWidth(self.titleLabel.frame) + CGRectGetWidth(self.imageView.frame) + theGap)) {
                CGRect frame = self.titleLabel.frame;
                frame.size.width = _limitWidth - (CGRectGetWidth(self.imageView.frame) - theGap);
                self.titleLabel.frame = frame;
            }
        }
    }
    if(_limitTextWidth > 0){
        [self.titleLabel setFrame:CGRectMake(self.titleLabel.frame.origin.x,
                                             self.titleLabel.frame.origin.x,
                                             self.limitTextWidth,
                                             self.titleLabel.frame.size.height)];
    }
    
    BOOL isH = (_imageLocation ==  kImageLocationLeft || _imageLocation == kImageLocationRight);
    CGSize r = CGSizeZero;
    if(_alignmentCenterMode == kAlignmentCenterModeUnion)
    {
        r = CGSizeMake(isH?(self.titleLabel.frame.size.width + theGap + self.imageView.frame.size.width) : (MAX(self.titleLabel.frame.size.width,self.imageView.frame.size.width))  ,
                       isH?(MAX(self.titleLabel.frame.size.height,self.imageView.frame.size.height)) : (self.titleLabel.frame.size.height + theGap + self.imageView.frame.size.height));
    }
    else if(_alignmentCenterMode == kAlignmentCenterModeTextCenter)
    {
        r = CGSizeMake(isH? (self.imageView.frame.size.width + theGap + self.titleLabel.frame.size.width/2) * 2 : MAX(self.imageView.frame.size.width, self.titleLabel.frame.size.width) , isH ? MAX(self.imageView.frame.size.height,self.titleLabel.frame.size.height) : ((self.imageView.frame.size.height + theGap + self.titleLabel.frame.size.height/2) * 2));
    }
    return r;
}


@end
