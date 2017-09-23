<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="HaselOne.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-4">
            <!-- BEGIN PROFILE SIDEBAR -->
            <div class="profile-sidebar">
                <!-- PORTLET MAIN -->
                <div class="portlet light profile-sidebar-portlet ">
                    <!-- SIDEBAR USERPIC -->
                    <div class="profile-userpic">
                        <img id="dashimgBigAvatar" runat="server" src="../Content/Images/media/profile/profile_user.jpg" class="img-responsive" alt="">
                    </div>
                    <!-- END SIDEBAR USERPIC -->
                    <!-- SIDEBAR USER TITLE -->
                    <div class="profile-usertitle">
                        <div class="profile-usertitle-job" id="dashposition" runat="server">Satış Müdürü </div>
                        <div class="profile-usertitle-job" id="dashlocation" runat="server">Orhanlı </div>
                        <div class="profile-usertitle-job"><a href="" id="dashemail" runat="server" class="lowercase">tugrul.caglar@hasel.com</a> </div>
                        <div class="profile-usertitle-job" id="dashphone" runat="server">+90 216 394 51 25 #1038 </div>
                        <div class="profile-usertitle-job" id="dashmobile" runat="server">+90 533 580 56 29 #2120 </div>
                    </div>
                    <!-- END SIDEBAR USER TITLE -->
                </div>
                <!-- END PORTLET MAIN -->
            </div>
            <!-- END BEGIN PROFILE SIDEBAR -->
        </div>
        <div class="col-md-8">
            <!-- BEGIN PROFILE CONTENT -->
            <div class="profile-content">
                <div class="row">
                    <div class="col-md-6">
                        <!-- PORTLET MAIN -->
                        <div class="portlet light ">
                            <div>
                                <div class="row number-stats margin-bottom-30">
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <div class="stat-left">
                                            <div class="stat-chart">
                                                <!-- do not line break "sparkline_bar" div. sparkline chart has an issue when the container div has line break -->
                                                <div id="sm-satis">
                                                    <canvas width="83" height="45" style="display: inline-block; width: 83px; height: 45px; vertical-align: top;"></canvas>
                                                </div>
                                            </div>
                                            <div class="stat-number">
                                                <div class="title">Satış </div>
                                                <div class="number">122 </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <div class="stat-right">
                                            <div class="stat-chart">
                                                <!-- do not line break "sparkline_bar" div. sparkline chart has an issue when the container div has line break -->
                                                <div id="sm-kiralama">
                                                    <canvas width="83" height="45" style="display: inline-block; width: 83px; height: 45px; vertical-align: top;"></canvas>
                                                </div>
                                            </div>
                                            <div class="stat-number">
                                                <div class="title">Kiralama </div>
                                                <div class="number">141 </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- STAT -->
                            <div>
                                <a href="/?m=myLists" class="icon-btn">
                                    <i class="fa fa-group"></i>
                                    <div>Ziyaret </div>
                                    <span class="badge badge-danger">2 </span>
                                </a>
                                <a href="/?m=myLists#collapse_2" class="icon-btn">
                                    <i class="fa fa-envelope"></i>
                                    <div>Talep </div>
                                    <span class="badge badge-danger">2 </span>
                                </a>
                                <a href="/?m=myLists#collapse_3" class="icon-btn">
                                    <i class="fa fa-file-text"></i>
                                    <div>Teklif </div>
                                    <span class="badge badge-success"><i class="fa fa-check" aria-hidden="true"></i></span>
                                </a>
                                <a href="/?m=myLists#collapse_4" class="icon-btn">
                                    <i class="fa fa-address-card"></i>
                                    <div>Carilerim </div>
                                    <span class="badge badge-danger">2 </span>
                                </a>
                                <a href="/?m=myLists#collapse_5" class="icon-btn">
                                    <i class="fa fa-question"></i>
                                    <div>Modül Y </div>
                                    <span class="badge badge-danger">3 </span>
                                </a>
                                <a href="/?m=myLists#collapse_6" class="icon-btn">
                                    <i class="fa fa-question"></i>
                                    <div>Modül Z </div>
                                    <span class="badge badge-success"><i class="fa fa-check" aria-hidden="true"></i></span>
                                </a>
                            </div>
                            <!-- END STAT -->
                        </div>
                        <!-- END PORTLET MAIN -->
                    </div>
                    <div class="col-md-6">
                        <div class="dashboard-stat2 ">
                            <div class="display">
                                <div class="number">
                                    <h3 class="font-green-sharp">
                                        <span data-counter="counterup" data-value="440">440</span>
                                        <small class="font-green-sharp">/ 580</small>
                                    </h3>
                                    <small>YILLIK ZİYARET</small>
                                </div>
                                <div class="icon">
                                    <i class="icon-pie-chart"></i>
                                </div>
                            </div>
                            <div class="progress-info">
                                <div class="progress">
                                    <span style="width: 76%;" class="progress-bar progress-bar-success green-sharp">
                                        <span class="sr-only">%76 Gerçekleşti</span>
                                    </span>
                                </div>
                                <div class="status">
                                    <div class="status-title">gerçekleşti </div>
                                    <div class="status-number">%76 </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="display: none;">
                    <div class="col-md-6">
                        <div class="portlet light portlet-fit ">

                            <div class="portlet-body">
                                <div class="timeline">
                                    <!-- TIMELINE ITEM -->
                                    <div class="timeline-item">
                                    </div>
                                    <!-- END TIMELINE ITEM -->
                                    <!-- TIMELINE ITEM WITH GOOGLE MAP -->
                                    <div class="timeline-item">
                                        <div class="timeline-badge">
                                            <img class="timeline-badge-userpic" src="../Content/Images/media/users/avatar80_7.jpg">
                                        </div>
                                        <div class="timeline-body">
                                            <div class="timeline-body-arrow"></div>
                                            <div class="timeline-body-head">
                                                <div class="timeline-body-head-caption">
                                                    <a href="javascript:;" class="timeline-body-title font-blue-madison">Paul Kiton</a>
                                                    <span class="timeline-body-time font-grey-cascade">Added office location at 2:50 PM</span>
                                                </div>
                                                <div class="timeline-body-head-actions">
                                                    <div class="btn-group">
                                                        <button class="btn btn-circle red btn-sm dropdown-toggle" type="button" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                                                            Actions
                                                                                <i class="fa fa-angle-down"></i>
                                                        </button>
                                                        <ul class="dropdown-menu pull-right" role="menu">
                                                            <li>
                                                                <a href="javascript:;">Action </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:;">Another action </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:;">Something else here </a>
                                                            </li>
                                                            <li class="divider"></li>
                                                            <li>
                                                                <a href="javascript:;">Separated link </a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="timeline-body-content">
                                                <div id="gmap_polygons" class="gmaps"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- END TIMELINE ITEM WITH GOOGLE MAP -->
                                    <!-- TIMELINE ITEM -->
                                    <div class="timeline-item">
                                        <div class="timeline-badge">
                                            <div class="timeline-icon">
                                                <i class="icon-user-following font-green-haze"></i>
                                            </div>
                                        </div>
                                        <div class="timeline-body">
                                            <div class="timeline-body-arrow"></div>
                                            <div class="timeline-body-head">
                                                <div class="timeline-body-head-caption">
                                                    <span class="timeline-body-alerttitle font-red-intense">You have new follower</span>
                                                    <span class="timeline-body-time font-grey-cascade">at 11:00 PM</span>
                                                </div>
                                                <div class="timeline-body-head-actions">
                                                    <div class="btn-group">
                                                        <button class="btn btn-circle green btn-outline btn-sm dropdown-toggle" type="button" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                                                            Actions
                                                                                <i class="fa fa-angle-down"></i>
                                                        </button>
                                                        <ul class="dropdown-menu pull-right" role="menu">
                                                            <li>
                                                                <a href="javascript:;">Action </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:;">Another action </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:;">Something else here </a>
                                                            </li>
                                                            <li class="divider"></li>
                                                            <li>
                                                                <a href="javascript:;">Separated link </a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="timeline-body-content">
                                                <span class="font-grey-cascade">You have new follower
                                                                        <a href="javascript:;">Ivan Rakitic</a>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- END TIMELINE ITEM -->
                                    <!-- TIMELINE ITEM -->
                                    <div class="timeline-item">
                                        <div class="timeline-badge">
                                            <div class="timeline-icon">
                                                <i class="icon-docs font-red-intense"></i>
                                            </div>
                                        </div>
                                        <div class="timeline-body">
                                            <div class="timeline-body-arrow"></div>
                                            <div class="timeline-body-head">
                                                <div class="timeline-body-head-caption">
                                                    <span class="timeline-body-alerttitle font-green-haze">Server Report</span>
                                                    <span class="timeline-body-time font-grey-cascade">Yesterday at 11:00 PM</span>
                                                </div>
                                                <div class="timeline-body-head-actions">
                                                    <div class="btn-group dropup">
                                                        <button class="btn btn-circle red btn-sm dropdown-toggle" type="button" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
                                                            Actions
                                                                                <i class="fa fa-angle-down"></i>
                                                        </button>
                                                        <ul class="dropdown-menu pull-right" role="menu">
                                                            <li>
                                                                <a href="javascript:;">Action </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:;">Another action </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:;">Something else here </a>
                                                            </li>
                                                            <li class="divider"></li>
                                                            <li>
                                                                <a href="javascript:;">Separated link </a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="timeline-body-content">
                                                <span class="font-grey-cascade">Lorem ipsum dolore sit amet
                                                                        <a href="javascript:;">Ispect</a>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- END TIMELINE ITEM -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END PROFILE CONTENT -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptbase" runat="server"></asp:Content>