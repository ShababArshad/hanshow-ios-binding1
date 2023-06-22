//
//  ViewController.m
//  
//
//  Created by hanshow on 2023/5/15.
//

#import "ViewController.h"
#import <HanshowNFCLib/HanshowNFCLib.h>
#import "NFCManager.h"
#import "UIImage+Category.h"

@interface ViewController ()
@property(nonatomic,strong) UIImageView *croppedImageView;
@end

#define WEAKSELF_DEFINE __weak __typeof(self)weakSelf = self;
#define RESOLUTION CGSizeMake(296.0, 152.0)

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
}

- (void)setupViews {
    
    self.view.backgroundColor = [UIColor whiteColor];
    

    
    self.infoButton = [SimpleButton buttonWithType:UIButtonTypeCustom];
    [_infoButton setImage:[[UIImage imageNamed:@"img_info"]resizedImage:CGSizeMake(50, 50) ] forState:UIControlStateNormal];
    [_infoButton setTitle:@"ESL ID" forState:UIControlStateNormal];
    [_infoButton setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [_infoButton.titleLabel setFont:[UIFont systemFontOfSize:16]];
    [_infoButton addTarget:self action:@selector(infoDidPressed) forControlEvents:UIControlEventTouchUpInside];
    [_infoButton setImageLocation:kImageLocationTop Gap:4];
    [_infoButton sizeToFit];
    
    [_infoButton setFrame:CGRectMake( 100 ,100, _infoButton.frame.size.width, _infoButton.frame.size.height)];
    
    [self.view addSubview:_infoButton];
    
}

-(void)viewDidLayoutSubviews{
    [super viewDidLayoutSubviews];
    if(_infoButton == nil){
        [self setupViews];
    }
}

- (void)infoDidPressed{
    [[NFCManager manager]getEslIdWithFinishBlock:^(NSString *eslId) {
        NSLog(@"eslId = %@",eslId);
    }];
}


@end
