package com.example.cavistahackerthon_rt.screens

import androidx.compose.animation.core.repeatable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AccountCircle
import androidx.compose.material.icons.filled.DateRange
import androidx.compose.material.icons.filled.Edit
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material.icons.filled.Notifications
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.Icon
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

@Composable
fun DashboardScreen() {
    Scaffold(
        bottomBar = {}
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(16.dp)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(paddingValues)
                    .padding(16.dp)
            ) {
                // top row profile
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Icon(
                        imageVector = Icons.Default.AccountCircle,
                        contentDescription = "Profile",
                        modifier = Modifier.size(40.dp)
                    )
                    Icon(
                        imageVector = Icons.Default.Notifications,
                        contentDescription = "Notification",
                        modifier = Modifier.size(32.dp)
                    )

                }
                Spacer(modifier = Modifier.height(32.dp))
                // Search bar
                OutlinedTextField(
                    value = "",
                    onValueChange = {},
                    leadingIcon = { Icon(Icons.Default.Search, contentDescription = "Search")},
                    placeholder = { Text("Search.....")},
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(50.dp),
                    shape = RoundedCornerShape(12.dp),
                    singleLine = true

                )
                Spacer(modifier = Modifier.height(16.dp))
                // Dashboard Grid
                Column {
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.SpaceBetween
                    ) {
                        DashboardCard("Open roles", "42", Icons.Default.Menu) // I need to replace this icon
                        DashboardCard("Applied roles", "10", Icons.Default.Edit)
                    }
                    Spacer(modifier = Modifier.height(12.dp))
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.SpaceBetween
                    ) {
                        DashboardCard("Interviews", "0", Icons.Default.Person)
                        DashboardCard("Applied roles", "10", Icons.Default.DateRange) // I need to replace this icon to GridView
                    }
                }
                Spacer(modifier = Modifier.height(20.dp))
                // Placeholder list items

            }
        }

    }

}