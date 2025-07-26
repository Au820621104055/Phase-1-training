package com.packageapp.dao;

import java.util.List;
import com.packageapp.model.Package;

public interface PackageDao {
    void addPackage(Package pkg);
    List<Package> getAllPackages();
    Package getPackageById(String id);
    void calculatePackageCost(Package pkg);
}