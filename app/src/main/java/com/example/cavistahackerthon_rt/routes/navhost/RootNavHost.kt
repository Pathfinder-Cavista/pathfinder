package com.example.cavistahackerthon_rt.routes.navhost

import androidx.compose.runtime.Composable
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import com.example.cavistahackerthon_rt.routes.AppRoutes
import com.example.cavistahackerthon_rt.screens.LoginScreen
import com.example.cavistahackerthon_rt.screens.SignUpScreen
import com.example.cavistahackerthon_rt.screens.WelcomeScreen

@Composable
fun RootNavHost(
    navController: NavHostController
) {
    NavHost(
        navController = navController,
        startDestination = AppRoutes.Route.WELCOME
    ) {
        composable(AppRoutes.Screen.Welcome.route) {
            WelcomeScreen(
                onLoginClick = {navController.navigate(AppRoutes.Route.LOGIN)},
                onSignUpClick = {navController.navigate(AppRoutes.Route.SIGNUP)}
            )
        }
        composable(AppRoutes.Screen.Login.route) {
            LoginScreen {  }
        }
        composable(AppRoutes.Screen.SignUp.route) {
            SignUpScreen()
        }
    }

}