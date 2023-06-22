//
//  ViewController.h
//  
//
//  Created by hanshow on 2023/5/15.
//

#import <UIKit/UIKit.h>
#import "SimpleButton.h"
@interface ViewController : UIViewController <UIImagePickerControllerDelegate, UINavigationControllerDelegate>

@property (nonatomic, strong) SimpleButton *infoButton;


@end
